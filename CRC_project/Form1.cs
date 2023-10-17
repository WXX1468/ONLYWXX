using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CRC_project
{
    public partial class Form1 : Form
    {

        [DllImport("crcdll.dll")]
        public static extern int add(int a,int b);
        [DllImport("crcdll.dll")]
        public static extern int test(int[] a, int b);
        [DllImport("crcdll.dll")]
        public static extern ushort crc(ushort[] data, int len);
        [DllImport("BMSDLL.dll")]
        public static extern bool Open(int index);
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] PEC_DATA = new byte[2];
             
            try
            {
                if (comboBox1.SelectedIndex == 0)
                {

                    byte[] data = new byte[2];
                    data[0] = Convert.ToByte(textBox1.Text, 16);
                    data[1] = Convert.ToByte(textBox2.Text, 16);
                    ushort[] PEC = new ushort[1];
                    PEC = ByteTOUshort(data);
                    ushort pec_data = crc1(PEC, 1);
                    PEC_DATA = UshortTOByte(pec_data);

                }
                else
                {
                    byte[] data = new byte[6];
                    data[0] = Convert.ToByte(textBox1.Text, 16);
                    data[1] = Convert.ToByte(textBox2.Text, 16);
                    data[2] = Convert.ToByte(textBox3.Text, 16);
                    data[3] = Convert.ToByte(textBox4.Text, 16);
                    data[4] = Convert.ToByte(textBox5.Text, 16);
                    data[5] = Convert.ToByte(textBox6.Text, 16);
                    ushort[] PEC = new ushort[3];
                    PEC = ByteTOUshort(data);
                    ushort pec_data = crc1(PEC, 3);
                    PEC_DATA = UshortTOByte(pec_data);
                }

                textBox7.Text = PEC_DATA[0].ToString("x2");
                textBox8.Text = PEC_DATA[1].ToString("x2");

            }
            catch
            {

            }
           
          

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
            {
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
                textBox6.Visible = false;
            }
            else
            {
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
                textBox6.Visible = true;
            }
        }
        void Input_limit(object sender)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                Convert.ToByte(tb.Text.Substring(tb.Text.Length - 1, 1), 16);

            }
            catch (Exception)
            {
                if (tb.Text.Length > 0)
                {
                    tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
                }

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Input_limit(sender);
        }
       
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Input_limit(sender);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Input_limit(sender);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Input_limit(sender);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Input_limit(sender);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            Input_limit(sender);
        }

        public ushort[] ByteTOUshort(byte[] data)
        {
            int len = data.Length;
            ushort[] CRC_Data = new ushort[len / 2];
            byte array;

            for (int i = 0; i < len; i += 2)
            {
                array = data[i];
                data[i] = data[i + 1];
                data[i + 1] = array;

            }
            for (int i = 0; i < len / 2; i++)
            {
                CRC_Data[i] = BitConverter.ToUInt16(data, 2 * i);
            }
            //    Console.WriteLine(" CRC_Data" + CRC_Data[0]);
            for (int i = 0; i < len; i += 2)
            {
                array = data[i];
                data[i] = data[i + 1];
                data[i + 1] = array;

            }
            return CRC_Data;
        }


        byte[] UshortTOByte(ushort udata)
        {
            byte[] UTB = new byte[2];
            UTB[1] = (byte)(udata | 0x00);
            UTB[0] = (byte)((udata >> 8) | 0x00);
            return UTB;
        }


        //pec计算
        public ushort crc1(ushort[] data, byte len)
        {
            ushort poly = 0x4599;
            ushort CRC = 0x0010;
            for (uint i = 0; i < len; i++)
            {
                for (UInt16 k = 0; k < 16; k++)
                {
                    if (((CRC & 0x4000) << 1) == (data[i] & 0x8000))
                    {
                        CRC = (ushort)(CRC << 1);
                        data[i] = (ushort)(data[i] << 1);
                        //    Console.WriteLine("CRC1 = " + CRC);

                    }
                    else
                    {
                        CRC = (ushort)(CRC << 1);
                        CRC = (ushort)(CRC ^ poly);
                        CRC = (ushort)(CRC & 0x7fff);
                        data[i] = (ushort)(data[i] << 1);
                        //   Console.WriteLine("CRC2 = " + CRC);                  
                    }

                }
            }

            CRC = (ushort)(CRC << 1);
            return CRC;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            /*ushort[] aaa = { 0x1122, 0x3344};

            ushort[] bbb = { 0x1122, 0x3344};
          
            ushort d = crc1(bbb, 2);
            ushort t = crc(aaa, 2);
            int n = add(1, 2);*/


            int[] arr = {2,1,3};
            int b = DPLIS(arr);
           /* BinaryTree tree = new BinaryTree(arr);
            tree.Frontshow(tree.rootNode);
            Console.WriteLine();
            tree.ordershow(tree.rootNode);
            Console.WriteLine();
            tree.PosShow(tree.rootNode);
            Console.WriteLine();*/
            
        }
       
        public int DPLIS(int[] a)
        {
            int sum = 0;
            int maxsum = 0;
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = i+1; j < a.Length; j++)
                {
                    if(a[i] < a[j])
                    {
                        sum++;
                        int newlen = a.Length - j;
                        int[] temp = new int[newlen];
                        Array.Copy(a, j, temp, 0, newlen);
                        DPLIS(temp);

                    }
                }
            }
            return sum;
        }
    }
    
    public class test
    {
        private int total_ic, pert;
        public void set_total_ic(int total_ic)
        {
            this.total_ic = total_ic;
        }
        public int get_total_ic()
        {
            return this.total_ic;
        }
        public int set_get_total_ic(int total_ic)
        {
            this.total_ic = total_ic;
            return this.total_ic;
        }
       
    }

    public class Node
    {

        public Node leftNode;
        public Node rightNode;
        public int item;
        public Node(int item)
        {

            this.item = item;
            leftNode = null;
            rightNode = null;
        }
    }

    public class BinaryTree
    {

        public  Node rootNode;

        public BinaryTree(int[] arr)
        {
            for(int i = 0; i < arr.Length; i++)
            {
                Add(arr[i]);
            }
        }

        private void Add(int item)
        {

            Node currNode = rootNode;

            if(rootNode == null)
            {
                rootNode = new Node(item);
                return;
            }

            while (true)
            {

                if(item < currNode.item)
                {

                    if(currNode.leftNode == null)
                    {
                        currNode.leftNode = new Node(item);
                        return;
                    }
                    else
                    {
                        currNode = currNode.leftNode;
                    }
                }
                else
                {
                    if(currNode.rightNode == null)
                    {

                        currNode.rightNode = new Node(item);
                        return;
                    }
                    else
                    {

                        currNode = currNode.rightNode;
                    }
                }

            }


        }


        public void Frontshow(Node rootNode)
        {

            if(rootNode != null)
            {

                Console.Write(rootNode.item);
                Frontshow(rootNode.leftNode);
                Frontshow(rootNode.rightNode);
            }
        }

        public void ordershow(Node rootNode)
        {

            if(rootNode != null)
            {

                ordershow(rootNode.leftNode);
                Console.Write(rootNode.item);
                ordershow(rootNode.rightNode);
            }
        }

        public void PosShow(Node rootNode)
        {
            if (rootNode != null)
            {
                PosShow(rootNode.leftNode);
                PosShow(rootNode.rightNode);
                Console.Write(rootNode.item);
            }
        }

    }
   
}
