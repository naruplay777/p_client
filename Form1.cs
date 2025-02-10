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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace p_client
{
    public partial class Form1 : Form
    {
        TcpClient client = new TcpClient();
        NetworkStream stream;
        private OpenFileDialog openFileDialog;

        public Form1()
        {
            InitializeComponent();
            client.Connect("127.0.0.1", 5000);
            stream = client.GetStream();
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) //Editar desconexion, no funcioa bien
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                SendFile(filePath);
                textBox1_TextChanged_1(sender, e);
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

        private void SendFile(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            stream.Write(fileData, 0, fileData.Length);
            MessageBox.Show("Archivo enviado", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

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
    }
}
