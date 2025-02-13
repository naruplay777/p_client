using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace p_client
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream stream;

        private OpenFileDialog openFileDialog;
        private Form2 form2;
        private System.Windows.Forms.Timer connectionTimer;
        private bool connectionLostMessageShown = false; // Bandera para controlar el mensaje

        public Form1(TcpClient client, NetworkStream stream, Form2 form2)
        {
            InitializeComponent();
            this.client = client;
            this.stream = stream;
            this.form2 = form2;
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt";

            connectionTimer = new System.Windows.Forms.Timer();
            connectionTimer.Interval = 500;
            connectionTimer.Tick += ConnectionTimer_Tick;
        }

        private void ConnectionTimer_Tick(object sender, EventArgs e)
        {
            _ = CheckConnection();
        }

        private async Task CheckConnection()
        {
            if (!await IsConnectionAliveAsync() && !connectionLostMessageShown)
            {
                connectionLostMessageShown = true; // Marcar que el mensaje ya ha sido mostrado
                connectionTimer.Stop();
                Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show("Conexión perdida. Volviendo a Form2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    form2.Show();
                });
            }
        }

        private async Task<bool> IsConnectionAliveAsync()
        {
            try
            {
                if (stream != null && stream.CanRead)
                {
                    byte[] buffer = new byte[1];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    return bytesRead > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en IsConnectionAliveAsync: {ex.Message}");
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Conectado al servidor", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            connectionTimer.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = openFileDialog.FileName;

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Por favor, seleccione un archivo antes de enviar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SendFile(filePath);
            textBox1_TextChanged_1(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (stream != null)
                {
                    string disconnectMessage = "Cliente desconectado";
                    byte[] data = Encoding.ASCII.GetBytes(disconnectMessage);
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar mensaje de desconexión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (client != null)
                {
                    client.Close();
                }
                MessageBox.Show("Conexión cerrada", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                form2.Show();
            }
        }

        private async void SendFile(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            await stream.WriteAsync(fileData, 0, fileData.Length);

            // Esperar la respuesta del servidor
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Procesar la respuesta del servidor
            string[] responseParts = response.Split('|');
            if (responseParts.Length >= 7 && responseParts[0] == "SOLICITUD_TERMINADA")
            {
                int queueNumber = int.Parse(responseParts[1]);
                int quantums = int.Parse(responseParts[2]);
                int operationsProcessed = int.Parse(responseParts[3]);
                int initialQueueNumber = int.Parse(responseParts[4]);
                string algorithm = responseParts[5];
                string fileContent = responseParts[6];

                // Mostrar la respuesta en textBox3
                Invoke((MethodInvoker)delegate
                {
                    textBox3.Text = $"Número en la cola: {queueNumber}\n" +
                                    $"Tiempo en quantums: {quantums}\n" +
                                    $"Operaciones procesadas: {operationsProcessed}\n" +
                                    $"Número inicial en la cola: {initialQueueNumber}\n" +
                                    $"Algoritmo aplicado: {algorithm}\n" +
                                    $"Contenido del archivo: {fileContent}";
                });
            }
            else
            {
                // Mostrar la respuesta en textBox3
                Invoke((MethodInvoker)delegate
                {
                    textBox3.Text = response;
                });
            }

            MessageBox.Show("Archivo enviado", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (openFileDialog.FileName != "") // Verificamos si se ha seleccionado un archivo
            {
                string filePath = openFileDialog.FileName;
                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length > 1)
                {
                    // Despues unimos todas las líneas excepto la primera (skip)
                    string contenido = string.Join(Environment.NewLine, lines.Skip(1));
                    textBox1.Text = contenido;
                }
                else
                {
                    textBox1.Text = string.Empty; // Limpiamos el contenido del textbox
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                textBox2.Text = Path.GetFileName(filePath);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
