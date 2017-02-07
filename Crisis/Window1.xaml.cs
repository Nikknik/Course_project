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
using System.Data.SqlClient;
using System.Windows.Markup;
using System.IO;


namespace Crisis
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        int i;
        string node1;
        
        public void Select(int i, string node)   //Метод, выполняющий запрос SELECT                      
        {
            SqlConnection conn = new SqlConnection("Data Source=NIK;Initial Catalog=Crisis;Integrated Security=True");
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ScreenForms, Items FROM " + node + " WHERE ID=@id ";
            SqlParameter par = new SqlParameter();
            par.ParameterName = "id";
            par.Value = i.ToString();
            cmd.Parameters.Add(par);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string s1 = reader.GetSqlString(0).ToString();            //Извлечение интерфейса из базы данных
                string s2 = reader.GetSqlString(1).ToString();            //Извлечение данных для заполнения экранной формы
                
                    try
                    {
                        File.WriteAllText(@"item2.txt", s2, Encoding.UTF8);
                        File.WriteAllText(@"ScreenForm2.txt", s1, Encoding.UTF8);
                       
                    }
                    catch (IOException)
                    {
                        
                    
                }
            }
            conn.Close();
        }

        public int Count(string node)    //Запрос COUNT
        {

            SqlConnection conn = new SqlConnection("Data Source=NIK;Initial Catalog=Crisis;Integrated Security=True");
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM " + node;
            int count;
            conn.Open();
            count = (int)cmd.ExecuteScalar();
            conn.Close();
            return (count);
        }

        public void Interface()
        {
            Button add = new Button();
            DataGrid Answ = new DataGrid();
            DependencyObject rootElement;
            using (FileStream fs = new FileStream(@"ScreenForm2.txt", FileMode.Open, FileAccess.Read))
            {
                rootElement = (DependencyObject)XamlReader.Load(fs);
            }
            string s = File.ReadAllText(@"ScreenForm2.txt", Encoding.UTF8);
            form2.Content = rootElement;
            if (s.Contains("add") == true)
            {
                add = (Button)LogicalTreeHelper.FindLogicalNode(rootElement, "add");
                add.Visibility = Visibility.Hidden;
            }
            Answ =(DataGrid)LogicalTreeHelper.FindLogicalNode(rootElement, "Answ");
            Answ.IsReadOnly=true;
        }


        public Window1(string node)
        {
           
            
                i = 1;
            Select(i, node);
            InitializeComponent();
            string appPath = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            data2.Source = new Uri(appPath + @"\item2.txt");
            Select(i, node);
            Interface();
            node1 = node;
            
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (i < Count(node1))
            {
                i = i + 1;
                Select(i, node1);
                Interface();
                data2.Refresh();
            }
        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            if (i > 1)
            {
                i = i - 1;
                Select(i, node1);
                Interface();
                data2.Refresh();
            }
        }
    }
}
