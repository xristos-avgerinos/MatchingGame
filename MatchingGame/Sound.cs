using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame
{
    //δημιουργια κλασης sound που θα περιεχει ολες τις μεθοδους που εχουν να κανουν με τους ηχους που θα ακουγονται στην εφαρμογη
    public class Sound
    {
        public AxWMPLib.AxWindowsMediaPlayer mediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
        
        public  bool enabled = false;
      
        public void startMusic()//μεθοδος που ενεργοποιει την μουσικη που θα ακουγεται οταν ξεκιναει η εφαρμογη
        {
            mediaPlayer.CreateControl();//δημιουργουμε το control
            mediaPlayer.settings.setMode("loop", true);
            mediaPlayer.settings.volume = 25;
            mediaPlayer.uiMode = "none";
            mediaPlayer.URL = "Krokotopia.mp3";
            enabled = true;
        }
        public void pauseMusic()//μεθοδος που σταματαει(pause) τη μουσικη
        {
            mediaPlayer.Ctlcontrols.pause();
            enabled = false;
        }
        public void PlayMusic()//μεθοδος που ξανα ενεργοποιει τη μουσικη
        {
            mediaPlayer.Ctlcontrols.play();
            enabled = true;
        }

        public void correct()//μεθοδος για τον ηχο που θα ακουγεται οταν θα βρισκουμε ενα "ταιριασμα" φωτογραφιων
        {
            if (enabled)
            {
                SoundPlayer player = new SoundPlayer("Correct.wav");
                player.Play();
            }
            
        }
          
        public void incorrect()//μεθοδος για τον ηχο που θα ακουγεται οταν δεν θα βρισκουμε ενα "ταιριασμα" φωτογραφιων
        {
            if (enabled)
            {
                SoundPlayer player = new SoundPlayer("Incorrect.wav");
                player.Play();
            }
            
        }
        public void End()
        {
            if (enabled)//μεθοδος για τον ηχο που θα ακουγεται οταν θα τελειωνουμε το παιχνιδι εχοντας βρει ολα τα "ταιριασματα" φωτογραφιων
            {
                SoundPlayer player = new SoundPlayer("End.wav");
                player.Play();
            }
            
           
            
        }

    }
}
