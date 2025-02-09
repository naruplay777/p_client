using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace p_client
{
    public partial class Form1 : Form
    {
        TcpClient client = new TcpClient();
        NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
            client.Connect("127.0.0.1", 5000);
            stream = client.GetStream();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (stream != null)
            {
                string disconnectMessage = "Cliente desconectado";
                byte[] data = Encoding.ASCII.GetBytes(disconnectMessage);
                stream.Write(data, 0, data.Length);

                stream.Close();
            }
            if (client != null)
            {
                client.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = textBox1.Text;
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);

            byte[] responseData = new byte[256];
            int bytes = stream.Read(responseData, 0, responseData.Length);
            string response = Encoding.ASCII.GetString(responseData, 0, bytes);
            label1.Text = response;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Archivos de texto (*.txt)|*.txt";
            openFileDialog1.Title = "Seleccionar archivo de texto";
            openFileDialog1.InitialDirectory = @"C:\MisDocumentos"; // Reemplaza con la ruta deseada

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string nombreArchivo = openFileDialog1.FileName;
                textBox1.Text = nombreArchivo;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (stream != null)
            {
                string disconnectMessage = "Cliente desconectado";
                byte[] data = Encoding.ASCII.GetBytes(disconnectMessage);
                stream.Write(data, 0, data.Length);

                stream.Close();
            }
            if (client != null)
            {
                client.Close();
            }
            MessageBox.Show("Conexión cerrada", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }

}
