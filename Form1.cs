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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }

}
