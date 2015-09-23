using System;
using System.Xml.Linq;

namespace FileSystem
{
    public class Control
    {
        private static XDocument xdoc = null;
        private static string root = null;
        private static string filename = null;

        public bool Open()
        {
            System.Console.WriteLine("\n>> Open/Create File\n");
            System.Console.Write("Plz type filename : ");
            filename = System.Console.ReadLine();
            try
            {
                xdoc = XDocument.Load(filename + ".xml");
                root = "root_" + filename;
            }
            catch (System.IO.FileNotFoundException e)
            {
                xdoc = new XDocument();
                root = "root_" + filename;
                xdoc.Add(new XElement(root));
                xdoc.Save(filename + ".xml");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                System.Console.WriteLine("Fail open/create file");
                return false;
            }

            if (xdoc.Element(root).IsEmpty)
            {
                System.Console.WriteLine("File is empty, setting column. enter '-1' to exit");
                XElement data = new XElement("data");
                do
                {
                    System.Console.Write("Type column name : ");
                    string column = System.Console.ReadLine();
                    if (column.Equals("-1")) break; // break while
                    data.Add(new XElement(column, column));

                } while (true);
                xdoc.Element(root).Add(data);
                xdoc.Save(filename + ".xml");
            }
            return true;
        }
        public bool Add()
        {
            if (xdoc == null) { System.Console.WriteLine("Plz Open/Create file first."); return false; }

            System.Console.WriteLine("\n>> Add data\n");


            XElement el = xdoc.Element(root).Element("data");

            XElement data = new XElement("data");

            foreach (XElement node in el.Nodes())
            {
                System.Console.Write("Set {0} : ", node.Name);
                data.Add(new XElement(node.Name, System.Console.ReadLine()));
            }

            xdoc.Element(root).Add(data);

            xdoc.Save(filename + ".xml");

            System.Console.WriteLine("\n>> Add data complete !!!\n");
            return true;
        }
        public bool Update()
        {
            if (xdoc == null) { System.Console.WriteLine("Plz Open/Create file first."); return false; }
            System.Console.WriteLine("\n>> Update data\n");

            XElement el = xdoc.Element(root).Element("data");

            System.Console.Write("Plz input {0} : ", ((XElement)el.FirstNode).Value);
            string search = System.Console.ReadLine();
            foreach (XElement data in el.Parent.Nodes())
            {
                // table column
                if (((XElement)data.FirstNode).Value.Equals("id"))
                {
                    foreach (XElement node in data.Nodes())
                    {
                        System.Console.Write("{0,-15}", node.Value);
                    }
                }
                if (((XElement)data.FirstNode).Value.Equals(search))
                {
                    foreach (XElement node in data.Nodes())
                    {
                        System.Console.Write("{0,-15}", node.Value);
                    }
                    foreach (XElement node in data.Nodes())
                    {
                        System.Console.Write("Set {0} : ", node.Name);
                        node.SetValue(System.Console.ReadLine());
                    }
                    break;
                }
                if (data.Equals(el.Parent.LastNode)) System.Console.WriteLine("No such data");
            }

            xdoc.Save(filename + ".xml");

            System.Console.WriteLine("\n>> Update data complete !!!\n");
            return true;
        }
        public bool Remove()
        {
            if (xdoc == null) { System.Console.WriteLine("Plz Open/Create file first."); return false; }
            System.Console.WriteLine("\n>> Remove data\n");

            XElement el = xdoc.Element(root).Element("data");

            System.Console.Write("Plz input {0} : ", ((XElement)el.FirstNode).Value);
            string search = System.Console.ReadLine();
            foreach (XElement data in el.Parent.Nodes())
            {
                if (((XElement)data.FirstNode).Value.Equals(search)) { data.Remove(); break; }
                if (data.Equals(el.Parent.LastNode)) System.Console.WriteLine("No such data");
            }

            xdoc.Save(filename + ".xml");

            System.Console.WriteLine("\n>> Remove data complete !!!\n");
            return true;
        }
        public bool Search()
        {
            if (xdoc == null) { System.Console.WriteLine("Plz Open/Create file first."); return false; }
            System.Console.WriteLine("\n>> Search data\n");

            XElement el = xdoc.Element(root).Element("data");

            System.Console.Write("Plz input {0} : ", ((XElement)el.FirstNode).Value);
            string search = System.Console.ReadLine();

            foreach (XElement data in el.Parent.Nodes())
            {
                // table column
                if (((XElement)data.FirstNode).Value.Equals("id"))
                {
                    foreach (XElement node in data.Nodes())
                    {
                        System.Console.Write("{0,-15}", node.Value);
                    }
                }
                if (((XElement)data.FirstNode).Value.Equals(search))
                {
                    foreach (XElement node in data.Nodes())
                    {
                        System.Console.Write("{0,-15}", node.Value);
                    }
                }
            }

            System.Console.WriteLine("\n>> Search data complete !!!\n");
            return true;
        }
        public bool List()
        {
            if (xdoc == null) { System.Console.WriteLine("Plz Open/Create file first."); return false; }
            System.Console.WriteLine("\n>> List\n");

            XElement el = xdoc.Element(root);

            foreach (XElement out_node in el.Nodes())
            {
                foreach (XElement node in out_node.Nodes())
                {
                    System.Console.Write("{0,-15}", node.Value);
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
            return true;
        }
    }
}