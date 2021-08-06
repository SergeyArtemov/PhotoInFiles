using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace PhotoInFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            //string iFile = "C:\\Photos1\\img1.jpg";
            //// Запись в таблицу ##TT1
            //byte[] im1;
            //FileInfo fInfo = new FileInfo(iFile);
            //long numBytes = fInfo.Length;
            //FileStream fStream = new FileStream(iFile, FileMode.Open, FileAccess.Read);
            //BinaryReader br = new BinaryReader(fStream);
            //im1 = br.ReadBytes((int)numBytes);
            //string iImageExtension = (Path.GetExtension(iFile)).Replace(".", "").ToLower();

            //using (SqlConnection sqlConnection = new SqlConnection(@"Server=192.168.0.14;Database=NeftmAzsk;Integrated Security = True")) // строка подключения к БД
            //{
            //    string commandText = "INSERT INTO ##TT1(Photo) VALUES(@ph)"; // запрос на вставку
            //    SqlCommand command = new SqlCommand(commandText, sqlConnection);
            //    command.Parameters.AddWithValue("@ph", (object)im1); // записываем само изображение
            //    //command.Parameters.AddWithValue("@screen_format", iImageExtension); // записываем расширение изображения
            //    sqlConnection.Open();
            //    command.ExecuteNonQuery();
            //    sqlConnection.Close();
            //}
            //long maxx = 295;
            //long maxx = 1295;
            long maxx0 = 0;
            long maxx = 330000;


            // Чтение из БД и запись в локальную папку
            SqlConnection conn1 = new SqlConnection("Server=192.168.0.14;Database=NeftmAzsk;Integrated Security = True");
            //using (SqlConnection conn1 = new SqlConnection("Server=192.168.0.14;Database=NeftmAzsk;Integrated Security = True")
            {  // Server= localhost; Database= employeedetails; Integrated Security=True;
                conn1.Open();
                string QueryOrders = String.Format($"SELECT [id],replace([FullName],''+char(9),'') fio, [Photo] FROM [NeftmAzsk].[dbo].[Users](NOLOCK) where Deleted = 0  and id <= {maxx} and id > {maxx0} order by id asc");
                //string QueryOrders = String.Format($"SELECT TOP (1) [Photo] FROM [##TT1](NOLOCK)");
                using (SqlCommand command = new SqlCommand(QueryOrders, conn1))
                {
                    //    int col;
                    //    GeocodeSearchResult searchResult;
                    String fio;
                    long lg;
                    int id;
                    var photo = new byte[1900000];
                    byte[] ph1 ;
                    string path = "C:\\Photos1\\";

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                    //reader.Read();
                    if (!reader.IsDBNull(2))
                    {
                        id = reader.GetInt32(0);
                        fio = reader.GetString(1);
                        lg = reader.GetBytes(2, 0, photo, 0, 1900000);
                        ph1 = new byte[lg];
                        Array.Copy(photo, ph1, lg);
                        //Console.WriteLine(fio);
                        //Console.WriteLine(lg);
                        //BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate));
                        //writer.Write(ph1);

                        //FileStream str = new FileStream(/*Path.Combine(ImageFilePath, IMAGE_NAME)*/ path, FileMode.Create);
                        //str.Write(ph1, 0, Convert.ToInt32(lg));

                        System.Drawing.Image newImage;

                        using (MemoryStream stream = new MemoryStream(ph1))
                        {
                            //using (newImage = System.Drawing.Image.FromStream(stream))
                            //{
                            newImage = System.Drawing.Image.FromStream(stream);
                            newImage.Save(path + fio +"_"+id.ToString() + ".jpg");

                            //}
                        }

                            Array.Clear(photo, 0, photo.Length);
                            Array.Clear(ph1, 0, ph1.Length);

                        } // if (!reader.IsDBNull(2))

                    }
                    reader.Close();
                    conn1.Close();
                }


                //using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                //{
                //    writer.Write(buf1);
                //}
                

                    //connection.Close();
                }
        }
    }
}


