using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

# For converting DNA to RNA

public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Visible = false;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Label1.Visible = true;
        string b = TextBox1.Text;
        string k = MinifyA(b);
        Label1.Text ="RNA is as"+"<Br>"+ k;
    }
    static string MinifyA(string p)
    {
        p = p.Replace("T", "U");
        return p;
    }
}

# For counting GC percentage

public partial class Default1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Visible = false;
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        Label1.Visible = true;
        string s1 = TextBox1.Text;
        int a = TextTool.CountStringOccurrences(s1, "A");
        int t = TextTool.CountStringOccurrences(s1, "T");
        int g = TextTool.CountStringOccurrences(s1, "G");
        int c = TextTool.CountStringOccurrences(s1, "C");
        int total = a + t + g + c;
        int gc = g + c;
        gc *= 100;
        double gcpercentage = (gc / total);
        Label1.Text = "<Br>" + "A=" + a.ToString() + "<Br>" + "T=" + t.ToString() + "<Br>" + "G=" + g.ToString() + "<Br>" + "C=" + c.ToString() + "<Br>" + "GC percentage= " + gcpercentage.ToString() + "%";
    }
    public static class TextTool
    {
        /// <summary> 
        /// Count occurrences of strings. 
        /// </summary>
        public static int CountStringOccurrences(string text, string pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }
    }
    }


# For converting DNA to protein

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Visible = false;
    }




    protected void Button1_Click(object sender, EventArgs e)
    {
        Label1.Visible = true;
        string b = TextBox1.Text;
        string k = MinifyA(b);
        Label1.Text ="The amino acids are:"+ "<Br>" + k;
    }
    static string MinifyA(string p)
    {
        p = p.Replace("T", "U");
        p = p.Replace("ATG", "M");
        p = p.Replace("ATC", "I");
        p = p.Replace("ATT", "I");
        p = p.Replace("ATA", "I");
        p = p.Replace("ACT", "T");
        p = p.Replace("ACC", "T");
        p = p.Replace("ACA", "T");
        p = p.Replace("ACG", "T");
        p = p.Replace("AAT", "N");
        p = p.Replace("AAC", "N");
        p = p.Replace("AAA", "K");
        p = p.Replace("AAG", "K");
        p = p.Replace("AGT", "S");
        p = p.Replace("AGC", "S");
        p = p.Replace("AGA", "R");
        p = p.Replace("AGG", "R");
        p = p.Replace("TTT", "F");
        p = p.Replace("TTC", "F");
        p = p.Replace("TTA", "L");
        p = p.Replace("TTG", "L");
        p = p.Replace("TCT", "S");
        p = p.Replace("TCC", "S");
        p = p.Replace("TCA", "S");
        p = p.Replace("TCG", "S");
        p = p.Replace("TAT", "Y");
        p = p.Replace("TAC", "Y");
        p = p.Replace("TGT", "C");
        p = p.Replace("TGC", "C");
        p = p.Replace("TGG", "W");
        p = p.Replace("CTT", "L");
        p = p.Replace("CTC", "L");
        p = p.Replace("CTA", "L");
        p = p.Replace("CTG", "L");
        p = p.Replace("CCT", "P");
        p = p.Replace("CCC", "P");
        p = p.Replace("CCA", "P");
        p = p.Replace("CCG", "P");
        p = p.Replace("CAT", "H");
        p = p.Replace("CAC", "H");
        p = p.Replace("CAA", "Q");
        p = p.Replace("CAG", "H");
        p = p.Replace("CGT", "R");
        p = p.Replace("CGC", "R");
        p = p.Replace("CGA", "R");
        p = p.Replace("CGG", "R");
        p = p.Replace("GTT", "V");
        p = p.Replace("GTC", "V");
        p = p.Replace("GTA", "V");
        p = p.Replace("GTG", "V");
        p = p.Replace("GCT", "A");
        p = p.Replace("GCC", "A");
        p = p.Replace("GCA", "A");
        p = p.Replace("GCG", "A");
        p = p.Replace("GAT", "D");
        p = p.Replace("GAC", "D");
        p = p.Replace("GAA", "E");
        p = p.Replace("GAG", "E");
        p = p.Replace("GGT", "G");
        p = p.Replace("GGC", "G");
        p = p.Replace("GGA", "G");
        p = p.Replace("GGG", "G");
        return p;
    }
}

# For converting RNA to Protein

public partial class Default3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Visible = false;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Label1.Visible = true;
        string b = TextBox1.Text;
        string k = MinifyA(b);
        Label1.Text = "The amino acids are"+ "<Br>"+k;
    }
    static string MinifyA(string p)
    {
        p = p.Replace("AUG", "M");
        p = p.Replace("AUC", "I");
        p = p.Replace("AUU", "I");
        p = p.Replace("AUA", "I");
        p = p.Replace("ACU", "T");
        p = p.Replace("ACC", "T");
        p = p.Replace("ACA", "T");
        p = p.Replace("ACG", "T");
        p = p.Replace("AAU", "N");
        p = p.Replace("AAC", "N");
        p = p.Replace("AAA", "K");
        p = p.Replace("AAG", "K");
        p = p.Replace("AGU", "S");
        p = p.Replace("AGC", "S");
        p = p.Replace("AGA", "R");
        p = p.Replace("AGG", "R");
        p = p.Replace("UUU", "F");
        p = p.Replace("UUC", "F");
        p = p.Replace("UUA", "L");
        p = p.Replace("UUG", "L");
        p = p.Replace("UCU", "S");
        p = p.Replace("UCC", "S");
        p = p.Replace("UCA", "S");
        p = p.Replace("UCG", "S");
        p = p.Replace("UAU", "Y");
        p = p.Replace("UAC", "Y");
        p = p.Replace("UGU", "C");
        p = p.Replace("UGC", "C");
        p = p.Replace("UGG", "W");
        p = p.Replace("CUU", "L");
        p = p.Replace("CUC", "L");
        p = p.Replace("CUA", "L");
        p = p.Replace("CUG", "L");
        p = p.Replace("CCU", "P");
        p = p.Replace("CCC", "P");
        p = p.Replace("CCA", "P");
        p = p.Replace("CCG", "P");
        p = p.Replace("CAU", "H");
        p = p.Replace("CAC", "H");
        p = p.Replace("CAA", "Q");
        p = p.Replace("CAG", "H");
        p = p.Replace("CGU", "R");
        p = p.Replace("CGC", "R");
        p = p.Replace("CGA", "R");
        p = p.Replace("CGG", "R");
        p = p.Replace("GUU", "V");
        p = p.Replace("GUC", "V");
        p = p.Replace("GUA", "V");
        p = p.Replace("GUG", "V");
        p = p.Replace("GCU", "A");
        p = p.Replace("GCC", "A");
        p = p.Replace("GCA", "A");
        p = p.Replace("GCG", "A");
        p = p.Replace("GAU", "D");
        p = p.Replace("GAC", "D");
        p = p.Replace("GAA", "E");
        p = p.Replace("GAG", "E");
        p = p.Replace("GGU", "G");
        p = p.Replace("GGC", "G");
        p = p.Replace("GGA", "G");
        p = p.Replace("GGG", "G");
        return p;
    }
}
