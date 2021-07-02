using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Puzzle_Proje
{
    public partial class Form1 : Form
    {
        int NullIndex, MoveIndex = 0;   //Boş picturebox ve hareket eden picturebox indexleri
        List<Bitmap> PictureList = new List<Bitmap>();   //Resimleri tutmak için liste. Bitmap sınıfı görseller üzerinde çalışabilmek için 
        int minute = 0, second = 0, splitsecond = 0;

        public Form1()
        {
            InitializeComponent();
            PictureList.AddRange(new Bitmap[] { Properties.Resources.Picture0, Properties.Resources.Picture1, Properties.Resources.Picture2,
                Properties.Resources.Picture3, Properties.Resources.Picture4,Properties.Resources.Picture5,Properties.Resources.Picture6,
                Properties.Resources.Picture7,Properties.Resources.Picture8, Properties.Resources._null}); // Resimleri tuttuğumuz listeye resimleri ekliyoruz
            MoveCount.Text += MoveIndex; // Hareket sayısını sayacak
        }
        private void PuzzleTemplate_Load(object sender, EventArgs e)
        {
            Mixing();   //Karıştırma işlemi
        }

        void Mixing()
        {
            do
            {
                int indx;   // Resimlerin rastgele gideceği index
                Random rnd = new Random();
                List<int> Indexes = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9 });  // 8 numaralı index son fotoğraf olduğu için yok
                for (int i = 0; i < 9; i++)
                {
                    Indexes.Remove((indx = Indexes[rnd.Next(0, Indexes.Count)]));  //Lisedeki i indexi kaldırıyor ki alt satırda resimleri rasgele yerleştirsin 
                    ((PictureBox)PuzzleTable.Controls[i]).Image = PictureList[indx];
                    if (indx == 9)
                        NullIndex = i; //boş resim
                }
            } while (CheckWin());
        }

        private void MixButton_Click(object sender, EventArgs e)
        {
            Mixing();
            MoveIndex = 0;
            MoveCount.Text = "Hamle Sayısı : 0";
        }

        // Kontrol metodu
        bool CheckWin()
        {
            int i;
            for (i = 0; i < 8; i++)  //Eğer sekiz fotoğrafın sekizi de doğru yerinde mi kontrol ettiriliyor.
            {
                if ((PuzzleTable.Controls[i] as PictureBox).Image != PictureList[i]) break;
            }
            if (i == 8)
                return true;
            else
                return false;
        }

        // Çıkış butonu
        private void QuitButton_Click(object sender, EventArgs e)
        {
            Quite(sender, e as FormClosingEventArgs);
        }

        // Çıkış penceresi 
        private void Quite(object sender, FormClosingEventArgs e)
        {
            DialogResult YN = MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Roket Puzzle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sender as Button != QuitButton && YN == DialogResult.No)
                e.Cancel = true;
            if (sender as Button == QuitButton && YN == DialogResult.Yes)
                Environment.Exit(0);
        }

        //Fotoğrafların değişimi sağlayan metot 
        private void ChangePictureBox(object sender, EventArgs e)
        {
            int PictureBoxIndex = PuzzleTable.Controls.IndexOf(sender as Control);
            if (NullIndex != PictureBoxIndex)
            {
                List<int> ContactBoxes = new List<int>(new int[] { ((PictureBoxIndex %3 == 0) ? - 1 : PictureBoxIndex - 1), PictureBoxIndex - 3,
                (PictureBoxIndex % 3 == 2) ? - 1 : PictureBoxIndex + 1, PictureBoxIndex + 3});   // Boş picturebox ile bağlantılı picturebox'ların tutulduğu list
                if (ContactBoxes.Contains(NullIndex))  // Bağlantılardan biri null picturebox ile bağlantılıysa 
                {
                    ((PictureBox)PuzzleTable.Controls[NullIndex]).Image = ((PictureBox)PuzzleTable.Controls[PictureBoxIndex]).Image; //Null picturebox ile
                    ((PictureBox)PuzzleTable.Controls[PictureBoxIndex]).Image = PictureList[9];                                      // doluyu değiştiriyor
                    NullIndex = PictureBoxIndex;
                    MoveCount.Text = "Hamle Sayısı : " + (++MoveIndex);
                    if (CheckWin())      // Eğer parçalar doğru yerleştirildiyse tebrikler ekranı çıkıyor ve puzzle sıfırlanıyor.
                    {
                        (PuzzleTable.Controls[8] as PictureBox).Image = PictureList[8];
                        MessageBox.Show("Tebrikler");
                        MoveIndex = 0;
                        MoveCount.Text = "Hamle Sayısı : 0";
                        Mixing();
                    }
                }
            }
        }
    }
}
