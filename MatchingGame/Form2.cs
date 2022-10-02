using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
        }

        //Continue Button
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text )) //αν δεν εχει δωσει ο χρηστης ονομα και πατησει το continue βγαζει μηνυμα σφαλματος
            {
                MessageBox.Show("Not Valid Username.Try again!");
            }
            else
            {
                User.Username = textBox1.Text ;
                this.Close();
             
            }
        }
        //Cancel Button.Μπορουμε να προχωρησουμε και χωρις να δωσουμε ονομα θετοντας ως default username το adventurer
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Exit Button
        private void button3_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
