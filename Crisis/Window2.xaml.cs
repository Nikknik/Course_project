using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Markup;

namespace Crisis
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        string Header1, Header2, Header3, s, node3;
        int j;
        string cell1, node21;
       

        public void Update(int i, string node)         //Запрос UPDATE
        {
            string s1 = File.ReadAllText(@"ScreenForm.txt", Encoding.UTF8);
            string s2 = File.ReadAllText(@"item.txt", Encoding.UTF8);
            SqlConnection conn = new SqlConnection("Data Source=NIK;Initial Catalog=Crisis;Integrated Security=True");
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE " + node + " SET ScreenForms='" + s1 + "', Items='" + s2 + "' WHERE ID=@id ";
            SqlParameter par = new SqlParameter();
            par.ParameterName = "id";
            par.Value = i.ToString();
            cmd.Parameters.Add(par);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

        }

        public Window2(string node2, string cell, string node1, int i)
        {
            InitializeComponent();

            this.Title = node1 + "." + cell + "." + node2;
            node3 = node1;
            j = i;
            cell1 = cell;
            node21 = node2;
            

            Header3 = "";

            XmlReader reader = XmlReader.Create("item.txt");
            reader.ReadToDescendant(node2);
            while (reader.Read())
            {
                if (reader.Name == "Header1")
                    Header1 = reader.ReadElementContentAsString();
                if (reader.Name == "Header2")
                    Header2 = reader.ReadElementContentAsString();
                if (reader.Name == "Header3")
                    Header3 = reader.ReadElementContentAsString();
            }
            reader.Close();

            
            column2.Header = Header2;
            column3.Header = Header3;

            string s = File.ReadAllText(@"item.txt", Encoding.UTF8);

            if(s.Contains(node2 + "." + cell) == false)
            {
               
                XmlDocument doc = new XmlDocument();
                doc.Load("item.txt");
                XmlNode element = doc.CreateElement(node2 + "." + cell);
                doc.DocumentElement.AppendChild(element);
                XmlNode answer = doc.CreateElement("answer");
                element.AppendChild(answer);
                XmlNode col1 = doc.CreateElement("column1");
                answer.AppendChild(col1);
                XmlNode col2 = doc.CreateElement("column2");
                answer.AppendChild(col2);
                doc.Save("item.txt");
                data4.Refresh();

                if (Header3!="")
                { 
                    
                    XmlNode col3 = doc.CreateElement("column3");
                    answer.AppendChild(col3);
                    doc.Save("item.txt");
                }
                else
                {
                    column3.Visibility = Visibility.Collapsed;
                }
                
                
                data4.Refresh();

                
            }

            

            string appPath = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            data4.Source = new Uri(appPath + @"\item.txt");

            data4.XPath = @"item/" + node2 + "." + cell;
            data4.Refresh();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            string appPath = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            data4.Source = new Uri(appPath + @"\item.txt");
            string source = data4.Source.LocalPath;
            data4.Document.Save(source);
            Update(j, node3);
        }


        private void add_Click(object sender, RoutedEventArgs e)
        {
            string appPath = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            data4.Source = new Uri(appPath + @"\item.txt");
            string source = data4.Source.LocalPath;
            data4.Document.Save(source);

            XmlDocument doc = new XmlDocument();
            doc.Load("item.txt");
            XmlNode element1=doc.SelectSingleNode(@"item/"+ node21 + "." + cell1);
            XmlNode answer = doc.CreateElement("answer");
            element1.AppendChild(answer);
            XmlNode col1 = doc.CreateElement("column1");
            answer.AppendChild(col1);
            XmlNode col2 = doc.CreateElement("column2");
            answer.AppendChild(col2);
            if (Header3 != "")
            {
                XmlNode col3 = doc.CreateElement("column3");
                answer.AppendChild(col3);
            }
            doc.Save("item.txt");
            data4.Refresh();

        }
    }
}
