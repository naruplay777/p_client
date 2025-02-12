﻿using System;
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

        private Task monitorConnectionTask;
        private OpenFileDialog openFileDialog;
        private Form2 form2; 

        public Form1(TcpClient client, NetworkStream stream, Form2 form2)
        {
            InitializeComponent();
            this.client = client;
            this.stream = stream;
            this.form2 = form2;
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt";
           
        }

        private async Task MonitorConnection()
        {
            try
            {
                while (client.Connected)
                {
                    // Intentamos leer de la corriente para ver si la conexión sigue activa
                    byte[] buffer = new byte[1];
                    try
                    {
                        // Si no podemos leer del stream, se ha perdido la conexión
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0) // Si no leemos nada, la conexión está cerrada
                        {
                            DisconnectClient();
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        // Si ocurre una excepción, significa que la conexión se ha perdido
                        DisconnectClient();
                        break;
                    }

                    // Espera un poco antes de volver a verificar
                    await Task.Delay(1000); // 1 segundo de espera
                }
            }
            catch (Exception ex)
            {
                // Si algo falla, desconectamos de forma segura
                DisconnectClient();
            }
        }

        private void DisconnectClient()
        {
                // Enviar un mensaje de desconexión (opcional)
                if (stream != null)
                {
                    string disconnectMessage = "Cliente desconectado";
                    byte[] data = Encoding.ASCII.GetBytes(disconnectMessage);
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }

                // Cerrar todo de forma segura
                if (client != null)
                {
                    stream.Close();
                    client.Close();
                }

                // Actualizar la interfaz de usuario
                this.Invoke((MethodInvoker)delegate
                {
                    form2.Show(); // Regresar al formulario principal
                    this.Close();  // Cerrar el formulario actual
                });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Conectado al servidor", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Iniciar la tarea para monitorear la conexión
            monitorConnectionTask = Task.Run(() => MonitorConnection());
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
                if (client != null)
                {
                    client.Close();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar la conexión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MessageBox.Show("Conexión cerrada", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            form2.Show();
        }

        private void SendFile(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            stream.Write(fileData, 0, fileData.Length);
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
            string filePath = openFileDialog.FileName;
            string contenido = File.ReadAllText(filePath);
            textBox1.Text = contenido;
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
    }
}
