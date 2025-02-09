using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void sendButton_Click(object sender, EventArgs e) //cambiar el nombre de la funcion por el nombre del boton para enviar el mensaje de ejemplo, un textbox para confirmar que la conexion se realizo correctamente
        {
            string message = messageTextBox.Text;
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);

            byte[] responseData = new byte[256];
            int bytes = stream.Read(responseData, 0, responseData.Length);
            string response = Encoding.ASCII.GetString(responseData, 0, bytes);
            responseLabel.Text = response;
        }

        private void closeButton_Click(object sender, EventArgs e) //cambiar el nombre de la funcion por el nombre del boton para cerrar la conección
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
    }
}
