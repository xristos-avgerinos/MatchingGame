using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        
        User user = new User();//δημιουργουμε ενα αντικειμενο της κλασης user για να εχουμε προσβαση στις non-static μεθοδους της
        Sound sound = new Sound();
        int time, mistakes = 0;
        bool GameOver;//boolean μεταβλητη για το αν εχει τελειωσει το παιχνιδι η οχι
        bool findingMatch = false;//boolean μεταβλητη για το αν εχουμε βρει ενα "ταιριασμα" η οχι.Μολις βρουμε τοτε τη ξανα κανουμε false 
        String lastPictureBoxClickedLoc;//το path του πρωτου picturebox που παταει ο χρηστης ωστε να ελεγχθει με το αμεσως επομενο και να βρουμε ενα ταιριασμα 
        String hiddenImage = "Photos/10.png"; //Η φωτο που θα εμφανιζεται οταν ειναι γυρισμενες οι φωτογραφιες δηλαδη κρυμενες
        PictureBox PreviousPictureBox;//η μεταβλητη αυτη κραταει το πρωτο picturebox που κανουμε κλικ καθε φορα ωστε να βρουμε ενα ταιριασμα

        public Form1()
        {
            InitializeComponent();
            
        }
        
        //Play Button
        private void button1_Click(object sender, EventArgs e)
        {
            //reset to default 
            findingMatch = false;
            lastPictureBoxClickedLoc = "";
            time = 0;
            mistakes = 0;
            foreach (PictureBox pictureBox in panel1.Controls.Cast<PictureBox>())
            {
                pictureBox.Visible = true;//κανουμε visible τις εικονες αλλα ακομα disabled ωστε να μην μπορει να πατησει πανω μεχρι να περασουν 3 δευτερολεπτα και να τις κρυψει
                pictureBox.Enabled = false;
            }
            //start
            timer1.Enabled = true;
            timer1.Start();
            shuffle();//κανουμε ανακατεμα των paths για τα pictureboxes ωστε να πανε σε τυχαιες θεσεις το καθε ενα
          
        }
        //Music Button
        private void button4_Click(object sender, EventArgs e)
        {
            if (sound.enabled == true)
            {
                sound.pauseMusic();
                button4.Text = "🔈";
            }
            else
            {
                sound.PlayMusic();
                button4.Text = "🔊";

            }

        }
        //Logout Button
        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
        //Rankings Button
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            sound.startMusic();//ξεκιναει να παιζει η background μουσικη
            
            timer2.Enabled = true; //ο timer που θα αλλαζει τις τιμες των labels καθε δευτερολεπτο


            if (DateTime.Now.Minute > 9)//αν ειναι πανω απο 9 τα λεπτα της συγκεκριμενης ωρας δηλ διψηφιος αριθμος εμφανιζουμε κανονικα την ωρα 
            {
                label4.Text = DateTime.Now.TimeOfDay.Hours.ToString() + ":" + DateTime.Now.TimeOfDay.Minutes.ToString();
            }
            else//αν ειναι μονοψηφιος αριθμος τα λεπτα τοτε εμφανιζουμε την ωρα με ενα μηδενικο μπροστα απο τα λεπτα
            {
                //8:05 instead of 8:5
                label4.Text = DateTime.Now.TimeOfDay.Hours.ToString() + ":0" + DateTime.Now.TimeOfDay.Minutes.ToString();
            }

            
            addPhotos(); // προσθετουμε τις default φωτος(paths) στην λιστα defaultImages
            label1.Text = "Time: " + time.ToString();//εμφανιζουμε τα δευτερολεπτα που παιζει το παιχνιδι ο χρηστης
            label3.Text = "Welcome to Tomb of Eternity  "+  User.Username;
            label5.Text = "Mistakes: " + mistakes.ToString();
            //κανουμε τα pictureboxes invisible και disabled μεχρι να πατησει ο χρηστης το Playbutton
            foreach (PictureBox pictureBox in panel1.Controls.Cast<PictureBox>())
            {
                pictureBox.Visible = false; //πριν πατησει ο χρηστης το play οι εικονες δεν φαινονται δηλ ειναι invisible
                pictureBox.Enabled = false; 

            }
            Form2 form2 = new Form2(); //μεταβαινουμε στη δευτερη φορμα που βαζει το username ο χρηστης 
            form2.ShowDialog();

        }

        async private void pictureBox_MouseDown(object sender, MouseEventArgs e) //ειναι async μεθοδος ωστε να μπορει να κανει await
        {
            
            PictureBox box = (PictureBox)sender;

            string index = box.Name.Remove(0,10);//κραταμε μονο τον αριθμο(index) του συγκεκριμενου picturebox που πατησε ο χρηστης
            box.ImageLocation = Program.imageList[Int32.Parse(index)-1];//θετει το path του συγκεκριμενου picturebox ισο με το path που εχει αποθηκευτει στην λιστα imagelist οταν εγινε το shuffle.Στην ουσια το γυρναει.

            //First  match
            if (findingMatch == false)//αν δεν εχουμε να ελεγξουμε για καποιο "ταιριασμα" δηλαδη δευτερο συνεχομενο mousedown
            {
                
                lastPictureBoxClickedLoc = box.ImageLocation; //αποθηκευουμε το path του συγκεκριμενου picturebox
                findingMatch = true; //θα κοιταξουμε το επομενο if στο επομενο mousedown για να βρουμε ενα ταιριασμα
                PreviousPictureBox = box; //αποθηκευουμε το συγκεκριμενο picturebox
                box.Enabled = false; // το κανουμε disable ωστε να μην μπορει ο χρηστης να το πατησει δυο φορες συνεχομενα και να εχουμε "ταιριασμα"
            }
            else if (box.ImageLocation.Equals(lastPictureBoxClickedLoc))//αν ειναι οι ιδιες φωτο που γυριστηκαν συνεχομενα
            {

                sound.correct();
                findingMatch = false; //το ξανα κανουμε false για να ψαξουμε στο επομενο mousedown καποιο επομενο "¨ταιριασμα"
                box.Enabled = false; //κανουμε disable και αυτο το picturebox ωστε μαζι με το προηγουμενο picturebox να μην μπορουν να ξανα πατηθουν

            }
            //Αν δεν εγινε ταιριασμα
            else
            {
                
                await Task.Delay(300); //Το await μπορει να χρησιμοποιηθει μονο μεσα σε async methonds και στην ουσια περιμενει μεχρι να ολοκληρωθει το συγκεκριμενο task που θα διαρκεσει 300 ms

                //κανουμε το συγκεκριμενο και το προγουμενο picturebox κρυμενα ξανα δινοντας τους το path του hiddenImage
                box.ImageLocation = hiddenImage;
                PreviousPictureBox.ImageLocation = hiddenImage;

                sound.incorrect();
                mistakes = mistakes + 1;
                PreviousPictureBox.Enabled = true;//ξανα ενεργοποιουμε το προηγουμενο picturebox που ηταν disabled για να μπορει να ξαναπατηθει
                findingMatch = false;              
            }

        }


        private void shuffle()//ολα τα pictureboxes στο panel1 παιρνουν μια τυχαια φωτο(path δηλαδη) απο τα path στη λιστα defaultImages
        {
            List<String> imageList = new List<string>();//τη χρησιμοποιουμε να να κανει update στο τελος της shuffle την imageList 
            Random random = new Random();
            List<int> randomlist = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            PictureBox[] boxes = { pictureBox1, pictureBox2,
                pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7,
                pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12 };
            
            for (int i = 0; i<boxes.Length; i++)
            {
                while (true) //loop μεχρι να δοθει ενας τυχαιος αριθμος απο τους 12 που δεν εχει ηδη δωθει 
                {
                    int myNumber = random.Next(0, 12);
                    if (randomlist.Contains(myNumber)) //Αν ειναι true αυτο σημανει οτι ο ταιχαιος αριθμος δεν εχει ξανα δωθει
                    {
                        boxes[i].ImageLocation = Program.defaultImages[myNumber]; //το Picture box διαλεγει μια τυχαια φωτογραφια(path) απο αυτες στο defaultImages
                        randomlist.Remove(myNumber);  //αφαιρουμε αυτον τον τυχαιο αριθμο απο την randomlist ωστε να μην μπορει να ξανα δωθει
                        imageList.Add(Program.defaultImages[myNumber]); //προσθετουμε αυτη τη φωτογραφια σε μια δευτερη λιστα και σε μια τυχαια θεση (0-11)
                        

                        break; //πηγαινουμε στο επομενο picturebox
                    }

                }

            }
            Program.imageList = imageList; //κανουμε ενημερωση της imageList 
        }

        private bool game_Over()
        {
            foreach (PictureBox pictureBox in panel1.Controls.Cast<PictureBox>())
            {
                if (pictureBox.ImageLocation != hiddenImage && time > 4)//αν εχουν περασει 4 δευτερολεπτα και ολες οι φωτος δεν ειναι γυρισμενες τοτε το παιχνιδι εχει τελειωσει
                {
                    //doSomething 
                }
                else
                {
                    return false;
                }

            }
            return true;
        }

        //add default photos to a list
        private void addPhotos()
        {
            Program.defaultImages.Add("Photos/Ra.png");        //0
            Program.defaultImages.Add("Photos/Ra.png");        //1
            Program.defaultImages.Add("Photos/Osiris.png");    //2
            Program.defaultImages.Add("Photos/Osiris.png");    //3
            Program.defaultImages.Add("Photos/Phoenix.png");   //4
            Program.defaultImages.Add("Photos/Phoenix.png");   //5
            Program.defaultImages.Add("Photos/Anubis.png");    //6
            Program.defaultImages.Add("Photos/Anubis.png");    //7
            Program.defaultImages.Add("Photos/Adventurer.png");//8
            Program.defaultImages.Add("Photos/Adventurer.png");//9
            Program.defaultImages.Add("Photos/Book.png");      //10
            Program.defaultImages.Add("Photos/Book.png");      //11
        }

        // if user choose to change photo
        private void changePhotos()
        {
            time = 0;
            mistakes = 0;
            shuffle();
        }



                      /*Timer Tick Events*/
        private void timer1_Tick(object sender, EventArgs e)
        {
            GameOver = game_Over();
            time = time + 1;
            label1.Text = "Time: " + time.ToString();
            
            if (time== 3) // οταν περασουν 3 δευτερολεπτα αφου πατησουμε το PlayButton κρυβουμε τις φωτογραφιες δινοντας τους το hiddenImage(path)
            {
                foreach (PictureBox pictureBox in panel1.Controls.Cast<PictureBox>())
                {
                    pictureBox.Enabled = true;
                    pictureBox.ImageLocation = hiddenImage;

                }
            }
            //ελεγχουμε αν εχει τελειωσει το παιχνιδι
            if (GameOver)
            {
                timer1.Enabled = false;
                sound.End();
                MessageBox.Show("Well Done "+User.Username+" You finished the game with "
                    +mistakes.ToString()+ " Mistakes in " +time.ToString()+" Seconds" );
                user.Insert(User.Username, time, mistakes, DateTime.Now.ToString());
                time = 0;
                mistakes = 0;
                
            }
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //Update time
            if(DateTime.Now.Minute> 9)
            {
                label4.Text = DateTime.Now.TimeOfDay.Hours.ToString() + ":" + DateTime.Now.TimeOfDay.Minutes.ToString();
            }
            else
            {
                //8:05 instead of 8:5
                label4.Text = DateTime.Now.TimeOfDay.Hours.ToString() + ":0" + DateTime.Now.TimeOfDay.Minutes.ToString();
            }
            //Update labels
            label1.Text = "Time: " + time.ToString();
            label3.Text = "Welcome to Tomb of Eternity  " + User.Username;
            label5.Text = "Mistakes: " + mistakes.ToString();

        }


                         /*Toolstrips*/
        private void boxToolStripMenuItem1_Click(object sender, EventArgs e)//αν επιλεξει να αλλαξει τη φωτο που εμφανιζεται στα pictureboxes οταν ειναι κρυμενα
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                hiddenImage = openFileDialog1.FileName;
                changePhotos();
            }
        }

        private void adventurerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.defaultImages[8] = openFileDialog1.FileName;
                Program.defaultImages[9] = openFileDialog1.FileName;
                changePhotos();
            }
        }

        private void bookToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.defaultImages[10] = openFileDialog1.FileName;
                Program.defaultImages[11] = openFileDialog1.FileName;
                changePhotos();
            }
        }

        private void raToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.defaultImages[0] = openFileDialog1.FileName;
                Program.defaultImages[1] = openFileDialog1.FileName;
                changePhotos();
            }
        }

        private void osirisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.defaultImages[2] = openFileDialog1.FileName;
                Program.defaultImages[3] = openFileDialog1.FileName;
                changePhotos();
            }
        }

        private void phoenixToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.defaultImages[4] = openFileDialog1.FileName;
                Program.defaultImages[5] = openFileDialog1.FileName;
                changePhotos();
            }
        }

        private void anubisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.defaultImages[6] = openFileDialog1.FileName;
                Program.defaultImages[7] = openFileDialog1.FileName;
                changePhotos();
            }
        }

        private void playToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            //reset
            findingMatch = false;
            lastPictureBoxClickedLoc = "";
            time = 0;
            mistakes = 0;
            foreach (PictureBox pictureBox in panel1.Controls.Cast<PictureBox>())
            {
                pictureBox.Visible = true;
                pictureBox.Enabled = false;
            }
            //start
            timer1.Enabled = true;
            timer1.Start();
            shuffle();
        }

        private void rankingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sound.pauseMusic();
            button4.Text = "🔈";
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sound.PlayMusic();
            button4.Text = "🔊";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by ThanosV, ChristosA, DimitrisP. "+Environment.NewLine+"~ Unipi 2020-2021");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gameplay: Match the pictures to win in the less possible mistakes/time." + Environment.NewLine +
                "Play Button: Start/Reset game."+Environment.NewLine+"Rankings Button: Show top 10 players."+Environment.NewLine+
                "🔊 Button: Music Enable/Disable."+Environment.NewLine+"Logout Button: Back to login form.");
        }

    }
}
