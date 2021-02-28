using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModyfikatorPlanu
{
    class Program
    {
        public static string[] toDelete;
        [STAThread]
        static void Main()
        {
            toDelete = File.ReadAllLines("./toDelete.txt");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "| *.HTML;| All files(*.*) | *.*";
            string text = null, path = null;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;
                text = File.ReadAllText(path, Encoding.GetEncoding(1250));

                int start = text.IndexOf("<table");
                int end = text.IndexOf("</table>");
                text = text.Substring(0, start) + Function(text.Substring(start, end - start + 8), false) +"<br>"+ Function(text.Substring(start, end - start + 8), true) + text.Substring(end + 8);

                for (int i = path.Length - 1; i > 0; i--)
                {
                    if (path[i] == '.')
                    {
                        path = path.Substring(0, i);
                    }
                }
                File.WriteAllText(path + "BETTER.html", text, Encoding.GetEncoding(1250));
                MessageBox.Show("The program has been finished.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static string Function(string text, bool evenWeek)
        {
            int a = text.IndexOf("<td");
            int b = text.IndexOf("</td>");
            bool endTag = true;
            if (text[a + 3] == '>')
                endTag = false;
            if (a == -1 || b == -1)
                return text;
            else
            {
                a += 4;
                text = text.Substring(0, a) + Middle(text.Substring(a, b - a + 5), endTag, evenWeek) + Function(text.Substring(b+5), evenWeek);
            }
            return text;
        }

        public static string Middle(string text, bool endTag, bool evenWeek) {
            bool doUsuniecia = false;
            foreach (string subject in toDelete)
                if (text.Contains(subject))
                    doUsuniecia = true;
            
            if(text.Contains(">parzyste") && text.Contains("nieparzyste"))
            {
                int a = text.IndexOf("<p>");
                int b = text.IndexOf("</p>");
                int c = text.LastIndexOf("</p>");
                if (evenWeek)
                    text = text.Remove(a, b - a + 4);
                else
                    text = text.Remove(b + 4, c - b + 2);
            }
            
            if (evenWeek && text.Contains("nieparzyste"))
                doUsuniecia = true;
            if (!evenWeek && text.Contains(">parzyste"))
                doUsuniecia = true;
            if (doUsuniecia)
            {
                text = "";
                if (endTag)
                    text = ">";
                return text + "</td>";
            }
            return text;
        }
    }
}
