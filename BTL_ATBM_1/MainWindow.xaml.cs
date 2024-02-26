using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using System.IO;
using Xceed.Words.NET;
using System.Windows.Xps.Packaging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Numerics;
using System.Linq;
using System.Buffers.Text;
//using Xceed.Document.NET;

namespace BTL_ATBM_1
{
    /// <summary>
    /// interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        string fileContent;

        private void readTxt(string filePath, Button btn)
        {
            fileContent = File.ReadAllText(filePath);
            //RichTextBox a = x as RichTextBox;
            if (btn.Name.ToString() == "layfile1")
            {
                vbky1.Document.Blocks.Clear();
                vbky1.Document.Blocks.Add(new Paragraph(new Run(fileContent)));
            }
            else if (btn.Name.ToString() == "layfile2")
            {
                vbky2.Document.Blocks.Clear();
                vbky2.Document.Blocks.Add(new Paragraph(new Run(fileContent)));
            }
            else if (btn.Name.ToString() == "layfile3")
            {
                vbchuky2.Clear();
                vbchuky2.Text = fileContent; 
            }
        }
        //private void readDocx(string filePath, Button btn)
        //{
        //    DocX document = DocX.Load(filePath);
        //    MemoryStream stream = new MemoryStream();
        //    document.SaveAs(stream);
        //    stream.Position = 0;
        //    TextRange range = new TextRange(vbky1.Document.ContentStart, vbky1.Document.ContentEnd);
        //    range.Load(stream, DataFormats.Rtf);
        //    document.Dispose();
        //}

        private void taokhoabtn_Click(object sender, RoutedEventArgs e)
        {
            taokhoa();
            string s_p = String.Format("p=" + p);
            string s_alpha = String.Format("α=" + alpha);
            string s_beta = String.Format("β=" + beta);
            public_key.Text = String.Format(s_p + "\n\n" + s_alpha + "\n\n" + s_beta);
            private_key.Text = String.Format("a=" + a);
            k_number.Text = String.Format("k=" + k);
        }

        private void layfile_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string filePath;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                string fileExtension = System.IO.Path.GetExtension(filePath);
                if (fileExtension == ".txt")
                {
                    //vbky1.Text = fileContent;
                    readTxt(filePath, btn);
                }
                else if (fileExtension == ".docx")
                {
                    //readDocx(filePath, btn);
                }
                else
                {
                    MessageBox.Show("Khong ho tro file nay");
                }
            }
            else
            {
                MessageBox.Show("Khong tim thay file");
            }
        }
        private void kybtn_Click(object sender, RoutedEventArgs e)
        {
            hamky();
            string s_gamma = String.Format("γ=" + gamma);
            string s_delta = String.Format("δ=" + delta);
            content_daky = String.Format(s_gamma + "\n" + s_delta);
            vbchuky1.Clear();
            vbchuky1.Text = content_daky;
        }

        private void chuyenbtn_Click(object sender, RoutedEventArgs e)
        {
            vbky2.Document.Blocks.Clear();
            TextRange tr = new TextRange(vbky2.Document.ContentEnd, vbky2.Document.ContentEnd);
            tr.Text = ndky;
            vbchuky2.Clear();
            vbchuky2.Text = content_daky;
        }

        private void luubtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Document";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                File.WriteAllText(filename, content_daky);
            }
            else
            {
                MessageBox.Show("Chua luu duoc file");
            }
        }

        private void ktbtn_Click(object sender, RoutedEventArgs e)
        {
            if (hamktchuky())
            {
                checktxt.Text = "Đúng";
            }
            else
            {
                checktxt.Text = "Sai";
            }
        }

        string content_daky;
        string ndky;
        BigInteger x;
        BigInteger p;
        BigInteger alpha;
        BigInteger a;
        BigInteger beta;
        BigInteger k;
        BigInteger gamma;
        BigInteger delta;

        BigInteger key_gamma;
        BigInteger key_delta;

        private void taokhoa()
        {
            do
            {
                p = LayNgauNhienSoNguyenLon(BigInteger.One << 257, BigInteger.One << 258);
                if (ktsnt(p)) break;
            } while (true);

            alpha = LayNgauNhienSoNguyenLon(BigInteger.One, p - 1);
            a = LayNgauNhienSoNguyenLon(new BigInteger(2), p - 2);
            beta = binhPhuongVaNhan(alpha, a, p);
            do
            {
                k = LayNgauNhienSoNguyenLon(1, p - 2);
                if (gcd(p - 1, k) == 1) break;
            } while (true);
        }

        private void hamky()
        {
            ndky = new TextRange(vbky1.Document.ContentStart, vbky1.Document.ContentEnd).Text;
            x = hambam(ndky);
            gamma = binhPhuongVaNhan(alpha, k, p);
            delta = ((x - a * gamma) * nghichdao(k, p - 1)) % (p - 1);
            if(delta < 0)
            {
                delta += (p - 1);
            }
        }

        bool hamktchuky()
        {
            string vbkt = new TextRange(vbky2.Document.ContentStart, vbky2.Document.ContentEnd).Text;
            BigInteger x1 = hambam(vbkt);
            string[] tachchuoil1 = vbchuky2.Text.Split("=");
            string[] tachchuoil2 = tachchuoil1[1].Split("δ");
            key_gamma = BigInteger.Parse(tachchuoil2[0].Trim());
            key_delta = BigInteger.Parse(tachchuoil1[2].Trim());
            beta = binhPhuongVaNhan(alpha, a, p);
            var kt1 = (binhPhuongVaNhan(beta, key_gamma, p) * binhPhuongVaNhan(key_gamma, key_delta, p)) % p;
            var kt2 = binhPhuongVaNhan(alpha, x1, p);
            if(kt1 == kt2)
            {
                return true;
            }
            return false;
        }

        BigInteger hambam(string s)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
            return new BigInteger(hashBytes.Concat(new byte[] { 0x00 }).ToArray());
        }

        BigInteger LayNgauNhienSoNguyenLon(BigInteger min, BigInteger max)
        {
            Random rand = new Random();
            byte[] bytes = max.ToByteArray();
            BigInteger result;
            do
            {
                rand.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F;
                result = new BigInteger(bytes);
            } while (result < min || result > max);
            return result;
        }

        bool ktsnt(BigInteger n, int k = 10)
        {
            if (n < 2)
                return false;

            foreach (int p in new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 })
            {
                if (n % p == 0)
                    return n == p;
            }

            BigInteger r = 0;
            BigInteger s = n - 1;

            while (s % 2 == 0)
            {
                r += 1;
                s /= 2;
            }

            for (int i = 0; i < k; i++)
            {
                BigInteger a = LayNgauNhienSoNguyenLon(2, n - 2);
                BigInteger x = binhPhuongVaNhan(a, s, n);

                if (x == 1 || x == n - 1)
                    continue;

                for (int j = 0; j < r - 1; j++)
                {
                    x = binhPhuongVaNhan(x, 2, n);

                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                    return false;
            }

            return true;
        }

        BigInteger nghichdao(BigInteger a, BigInteger m)
        {
            BigInteger z = m;
            BigInteger r, q, y = 0, y0 = 0, y1 = 1;
            while (a > 0)
            {
                r = m % a;
                if (r == 0) break;
                q = m / a;
                y = y0 - y1 * q;
                m = a;
                a = r;
                y0 = y1;
                y1 = y;
            }
            if (a > 1)
            {
                return -1;
            }
            else
            {
                if (y > 0) return y;
                else
                {
                    while (y < 0)
                    {
                        y += z;
                    }
                    return y;
                }
            }
        }

        BigInteger gcd(BigInteger a, BigInteger b)
        {
            BigInteger r;
            if (a == 0)
                return b;
            if (b == 0)
                return a;
            while (b > 0)
            {
                r = a % b;
                a = b;
                b = r;
            }
            return a;
        }

        BigInteger binhPhuongVaNhan(BigInteger x, BigInteger n, BigInteger m)
        {
            BigInteger kq = 1;
            var a = new List<BigInteger>();
            while (n > 0)
            {
                a.Add(n % 2);
                n /= 2;
            }
            for (int i = a.Count - 1; i >= 0; i--)
            {
                kq = (kq * kq) % m;
                if (a[i] == 1)
                {
                    kq = (kq * x) % m;
                }
            }
            return kq;
        }

        private void clear_btn_Click(object sender, RoutedEventArgs e)
        {
            public_key.Clear();
            private_key.Clear();
            k_number.Clear();
            vbky1.Document.Blocks.Clear();
            vbky2.Document.Blocks.Clear();
            vbchuky1.Clear();
            vbchuky2.Clear();
            checktxt.Clear();
        }
    }
}
