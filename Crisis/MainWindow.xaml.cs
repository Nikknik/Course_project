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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Markup;
using System.Data.SqlClient;
using System.Xml;


namespace Crisis
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string node, node1, node2, s1;                           //Строка, содержащая название выбранного узда TreeView
        int i,j;
        string cont;
        string ConnectionString= "Data Source=NIK;Initial Catalog=Report_Test;Integrated Security=True";

        public void TreeViewItems()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Section, MainSection FROM Sections";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.Header = reader.GetSqlString(0);
                treeViewItem.MouseDoubleClick += MouseDob;
                if (reader.GetSqlString(1) =="Ситуационный анализ")
                    SituationAn.Items.Add(treeViewItem);
                else
                    FinancialAn.Items.Add(treeViewItem);
            }
            conn.Close();
        }

        public void Select()   //Метод, выполняющий запрос SELECT                      
        {
            SqlConnection conn=new SqlConnection(ConnectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT Questions.ID, Questions.ScreenForm, Questions.Item FROM Questions 
                                WHERE ID=(SELECT MIN(Questions.ID)  FROM Questions WHERE ID>=@id AND Section_ID IN 
                                (SELECT Sections.ID FROM Sections WHERE Section='"+node+"'))";
            SqlParameter par = new SqlParameter();
            par.ParameterName = "id";
            par.Value = i.ToString();
            cmd.Parameters.Add(par);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                i = reader.GetInt32(0);
                s1 = reader.GetSqlString(1).ToString();            //Извлечение интерфейса из базы данных
                string s2 = reader.GetSqlString(2).ToString();            //Извлечение данных для заполнения экранной формы
                File.WriteAllText(@"item.txt", s2, Encoding.UTF8);
                File.WriteAllText(@"ScreenForm.txt", s1, Encoding.UTF8);   
                
            }
            conn.Close();
            
        }

        public void Update()         //Запрос UPDATE
        {
            s1 = File.ReadAllText(@"ScreenForm.txt", Encoding.UTF8);
            
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Questions SET Item='" + File.ReadAllText(@"item.txt", Encoding.UTF8) + "' WHERE ID=@id ";
            SqlParameter par = new SqlParameter();
            par.ParameterName = "id";
            par.Value = i.ToString();
            cmd.Parameters.Add(par);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

        }

        public int Count()    //Запрос COUNT
        {
            
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(*) FROM Questions WHERE 
                               Section_ID IN (SELECT Sections.ID FROM Sections WHERE Section='" + node + "')";
            int count;
            conn.Open();
            count = (int)cmd.ExecuteScalar();
            conn.Close();
            textBox.Text = count.ToString();
            return (count);
        } 

        

       public void Interface()
        {
            
           
            DataGrid Answ = new DataGrid();
            Button add = new Button();
            MenuItem MenuItem1 = new MenuItem();
            MenuItem MenuItem2 = new MenuItem();
            
            DependencyObject rootElement;
            rootElement = (DependencyObject)XamlReader.Parse(s1);
            ScreenForm.Content = rootElement;
            if (s1.Contains("add_Column") == true)
            {
                add = (Button)LogicalTreeHelper.FindLogicalNode(rootElement, "add_Column");
                add.Click += add_Click;
            }

            if (s1.Contains("MenuItem1") == true)
            {
                Answ = (DataGrid)LogicalTreeHelper.FindLogicalNode(rootElement, "Answ");
                Answ.MouseRightButtonUp += hj;
                MenuItem1 = (MenuItem)Answ.FindName("MenuItem1");
                MenuItem1.Click += ContextMenu2_Click;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            TreeViewItems();
            string appPath = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            data.Source = new Uri(appPath + @"\item.txt");
            string source = data.Source.LocalPath;
            
            Binding bd = new Binding();
            bd.XPath = "column1";
            column1.Binding = bd;


        }

        public void MouseDob(object sender, MouseButtonEventArgs e)     //обработчик TreeView (выбор названия раздела)
        {
            TreeViewItem sect = (TreeViewItem)treeView.SelectedItem;
            node = sect.Header.ToString();
            i = 1;
            Select();
            Interface();
            data.Refresh();
            j = 1;  
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
             if (j < Count())
            {
                i = i + 1;
                Select();
                Interface();
                data.Refresh();
                j = j + 1;
            }
        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            if (j > 1)
            {
                i = i - 1;
                Select();
                Interface();
                data.Refresh();
                j = j - 1;
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            string appPath = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            data.Source = new Uri(appPath + @"\item.txt");
            string source = data.Source.LocalPath;
            data.Document.Save(source);
            Update();
            data.Refresh();  
        }

        public void add_Click(object sender, RoutedEventArgs e)
        {

            string appPath = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            data.Source = new Uri(appPath + @"\item.txt");
            string source = data.Source.LocalPath;
            data.Document.Save(source);
            XmlDocument doc = new XmlDocument();
            doc.Load("item.txt");
            XmlNode answer = doc.SelectSingleNode("item/answer");
            int chislo = answer.ChildNodes.Count;
            XmlNode element = doc.CreateElement("answer");
            doc.DocumentElement.AppendChild(element);
            XmlNode col1 = doc.CreateElement("column1");
            element.AppendChild(col1);
            XmlNode col2 = doc.CreateElement("column2");
            element.AppendChild(col2);
            doc.Save("item.txt");
            XmlNode col;
            for (int n=3; n<=chislo; n++)
            {
                col = doc.CreateElement("column" + n.ToString());
                element.AppendChild(col);
                doc.Save("item.txt");
            }
            data.Refresh();
        }

        public void hj(object sender, MouseButtonEventArgs e)
        {
            DataGrid Answ = (DataGrid)sender;


            var ci = new DataGridCellInfo(Answ.CurrentItem, Answ.Columns[1]);
            var content = ci.Column.GetCellContent(ci.Item) as TextBlock;
            cont = content.Text;
            
        }

        private void Answ_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void MouseUp1(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem tv = (TreeViewItem)sender;
            node1 = tv.Header.ToString();
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
           new Window1(node1).Show();
          
           
        }

        private void ContextMenu2_Click(object sender, RoutedEventArgs e)
        {


            string cell = cont;
            
            MenuItem mn = (MenuItem)sender;
            node2 = mn.Header.ToString();
            new Window2(node2, cell, node, i).Show();
            textBox.Text = node2;
        }
    }
}
