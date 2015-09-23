using System;
using System.Xml.Linq;

namespace FileSystem
{
    public class Control
    {
        private static XDocument[] xdoc = null;
        private static string root = null;
        private static string filename = null;

        public bool Open()
        {
            System.Console.WriteLine("\n>> Open/Create File\n");
            System.Console.Write("Plz type filename : ");
            filename = System.Console.ReadLine();
            xdoc = new XDocument[6];
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    xdoc[i] = XDocument.Load(filename + "_" + i + ".xml");
                }
                root = "root_" + filename;
            }
            catch (System.IO.FileNotFoundException e)
            {
                for (int i = 0; i < 6; i++)
                {
                    xdoc[i] = new XDocument();
                    root = "root_" + filename;
                    xdoc[i].Add(new XElement(root));
                    xdoc[i].Save(filename + "_" + i + ".xml");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                System.Console.WriteLine("Fail open/create file");
                return false;
            }

            if (xdoc[0].Element(root).IsEmpty)
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
                xdoc[0].Element(root).Add(data);
                xdoc[0].Save(filename + "_0.xml");
            }
            return true;
        }

        public bool Add()
        {
            if (xdoc == null) { System.Console.WriteLine("Plz Open/Create file first."); return false; }

            System.Console.WriteLine("\n>> Add data\n");

            // table scheme from file 0
            XElement el = xdoc[0].Element(root).Element("data");

            XElement data = new XElement("data");

            int filenum = -1;
            int count = 0;

            foreach (XElement node in el.Nodes())
            {
                System.Console.Write("Set {0} : ", node.Name);
                data.Add(new XElement(node.Name, System.Console.ReadLine()));
                if (node.Name.ToString().Equals("id")) filenum = Convert.ToInt32(data.Value) % 5;
            }

            foreach (XElement node in xdoc[filenum].Element(root).Nodes()) count++; // data count
            if (count > ((filenum == 0) ? 5 : 4)) // overflow
            {
                xdoc[5].Element(root).Add(data);
                xdoc[5].Save(filename + "_5.xml");
            }
            else // save original
            {
                xdoc[filenum].Element(root).Add(data);
                xdoc[filenum].Save(filename + "_" + filenum + ".xml");
            }
            System.Console.WriteLine("\n>> Add data complete !!!\n");
            return true;
        }

        public bool Update()
        {
            if (xdoc == null) { System.Console.WriteLine("Plz Open/Create file first."); return false; }
            System.Console.WriteLine("\n>> Update data\n");

            // table scheme from file 0
            XElement el = xdoc[0].Element(root).Element("data");

            // input search id num
            System.Console.Write("Plz input {0} : ", ((XElement)el.FirstNode).Value);
            string search = System.Console.ReadLine();
            int filenum = Convert.ToInt32(search) % 5;

            // table column
            foreach (XElement node in el.Nodes())
            {
                System.Console.Write("{0,-10} | ", node.Value);
            }
            System.Console.WriteLine();

            int found = 0;
            // original
            el = xdoc[filenum].Element(root);
            foreach (XElement data in el.Nodes())
            {
                if (((XElement)data.FirstNode).Value.Equals(search))
                {
                    // printf present value
                    foreach (XElement node in data.Nodes())
                    {
                        System.Console.Write("{0,-10} | ", node.Value);
                    }
                    // input update value
                    foreach (XElement node in data.Nodes())
                    {
                        if (node.Name.ToString().Equals("id"))
                            continue;
                        System.Console.Write("Set {0} : ", node.Name);
                        string change_value = System.Console.ReadLine();

                        node.SetValue((change_value.Length < 1) ? node.Value : change_value);
                    }
                    found = 1; // update found
                    break;
                }
            }
            if (found == 0)
            {
                // overflow
                el = xdoc[5].Element(root);
                foreach (XElement data in el.Nodes())
                {
                    if (((XElement)data.FirstNode).Value.Equals(search))
                    {
                        // printf present value
                        foreach (XElement node in data.Nodes())
                        {
                            System.Console.Write("{0,-10} | ", node.Value);
                        }
                        System.Console.WriteLine();
                        // input update value
                        foreach (XElement node in data.Nodes())
                        {
                            if (node.Name.ToString().Equals("id"))
                                continue;
                            System.Console.Write("Set {0} : ", node.Name);
                            string change_value = System.Console.ReadLine();

                            node.SetValue((change_value.Length < 1) ? node.Value : change_value);
                        }
                        found = 2; // update found in overflow
                        break;
                    }
                }
            }

            // update result
            if (found == 0) System.Console.WriteLine("No such data");
            else if (found == 1) xdoc[filenum].Save(filename + "_" + filenum + ".xml");
            else if (found == 2) xdoc[5].Save(filename + "_5.xml");

            System.Console.WriteLine("\n>> Update data complete !!!\n");
            return true;
        }

        public bool Remove()
        {
            if (xdoc == null) { System.Console.WriteLine("Plz Open/Create file first."); return false; }
            System.Console.WriteLine("\n>> Remove data\n");

            // table scheme from file 0
            XElement el = xdoc[0].Element(root).Element("data");

            // input remove id num
            System.Console.Write("Plz input {0} : ", ((XElement)el.FirstNode).Value);
            string search = System.Console.ReadLine();
            int filenum = Convert.ToInt32(search) % 5;

            int found = 0;
            // original
            el = xdoc[filenum].Element(root);
            foreach (XElement data in el.Nodes())
            {
                if (((XElement)data.FirstNode).Value.Equals(search))
                {
                    data.Remove();
                    found = 1; // remove found
                    break;
                }
            }
            if (found == 0)
            {
                // overflow
                el = xdoc[5].Element(root);
                foreach (XElement data in el.Nodes())
                {
                    if (((XElement)data.FirstNode).Value.Equals(search))
                    {
                        data.Remove();
                        found = 2; // remove found in overflow
                        break;
                    }
                }
            }

            // remove result
            if (found == 0) System.Console.WriteLine("No such data");
            else if (found == 1) xdoc[filenum].Save(filename + "_" + filenum + ".xml");
            else if (found == 2) xdoc[5].Save(filename + "_5.xml");

            System.Console.WriteLine("\n>> Remove data complete !!!\n");
            return true;
        }

        public bool Search()
        {
            if (xdoc == null) { System.Console.WriteLine("Plz Open/Create file first."); return false; }
            System.Console.WriteLine("\n>> Search data\n");

            // table scheme from file 0
            XElement el = xdoc[0].Element(root).Element("data");

            // input search id num
            System.Console.Write("Plz input {0} : ", ((XElement)el.FirstNode).Value);
            string search = System.Console.ReadLine();
            int filenum = Convert.ToInt32(search) % 5;

            // table column
            foreach (XElement node in el.Nodes())
            {
                System.Console.Write("{0,-10} | ", node.Value);
            }
            System.Console.WriteLine();

            int found = 0;
            // original
            el = xdoc[filenum].Element(root);
            foreach (XElement data in el.Nodes())
            {
                if (((XElement)data.FirstNode).Value.Equals(search))
                {
                    foreach (XElement node in data.Nodes())
                    {
                        System.Console.Write("{0,-10} | ", node.Value);
                    }
                    System.Console.WriteLine();
                    found = 1; // search found
                    break;
                }
            }
            if (found == 0)
            {
                // overflow
                el = xdoc[5].Element(root);
                foreach (XElement data in el.Nodes())
                {
                    if (((XElement)data.FirstNode).Value.Equals(search))
                    {
                        foreach (XElement node in data.Nodes())
                        {
                            System.Console.Write("{0,-10} | ", node.Value);
                        }
                        System.Console.WriteLine();
                        found = 2; // remove found in overflow
                        break;
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

            // print file 0~4 with overflow(5)
            for (int filenum = 0; filenum < 5; filenum++)
            {
                XElement el = xdoc[filenum].Element(root);

                foreach (XElement data in el.Nodes())
                {
                    foreach (XElement node in data.Nodes())
                    {
                        System.Console.Write("{0,-10} | ", node.Value);
                    }
                    System.Console.WriteLine();
                }

                el = xdoc[5].Element(root);

                foreach (XElement data in el.Nodes())
                {
                    //System.Console.WriteLine(Convert.ToInt32(((XElement)data.FirstNode).Value) == filenum);
                    if ((Convert.ToInt32(((XElement)data.FirstNode).Value) % 5) == filenum)
                    {
                        foreach (XElement node in data.Nodes())
                        {
                            System.Console.Write("{0,-10} | ", node.Value);
                        }
                        System.Console.WriteLine();
                    }
                }
            }
            System.Console.WriteLine();
            return true;
        }
    }
}