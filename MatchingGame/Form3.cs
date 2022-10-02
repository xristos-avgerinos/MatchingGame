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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            User user = new User();
            label1.Text = user.Read();
        }
        //Go Back Button
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
