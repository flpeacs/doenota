using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Data.Linq;
using System.Windows;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Info;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using Windows.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace NotaFiscal
{
    
    class Upload
    {
        DB2 dados;

        public byte[] ImageToArray(BitmapImage imageUpload)
        {
            imageUpload.CreateOptions = BitmapCreateOptions.None;
            WriteableBitmap wbmp = new WriteableBitmap(imageUpload);
            MemoryStream ms = new MemoryStream();
            wbmp.SaveJpeg(ms, 1280, 1920, 0, 90);

            return ms.ToArray();
        }

        private async void insertIntoMobileApp()
        {
            try
            {
                while (true)
                {
                    IMobileServiceTable<images_data> table = App.MobileService.GetTable<images_data>();
                    images_data image_data = new images_data();
                    await table.InsertAsync(image_data);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public bool uploadServer(BitmapImage imageUpload)
        {
            try
            {
                DB id = new DB();

                using (DB2DataContext db = new DB2DataContext(DB2DataContext.DB2ConnectionString))
                {
                    DB2 dados = new DB2();

                    dados.File = ImageToArray(imageUpload);

                    using (DBDataContext dbAux = new DBDataContext(DBDataContext.DBConnectionString))
                    {
                        int count = (from tarefa in dbAux.DBItems where tarefa.Selected == true select tarefa).Count();

                        if (count > 0)
                            id = (from tarefa in dbAux.DBItems where tarefa.Selected == true select tarefa).First();
                        else
                        {
                            Random rnd = new Random();
                            ObservableCollection<DB> col = new ObservableCollection<DB>(from tarefa in dbAux.DBItems select tarefa);
                            id = col.ElementAt(rnd.Next(col.Count));
                        }
                    }

                    dados.Institution_Id = id.Id;

                    db.DB2Items.InsertOnSubmit(dados);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }


        
        public async Task UploadService() {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));

                try
                {
                    var currentList = new NetworkInterfaceList().Where(i => i.InterfaceState == ConnectState.Connected).Select(i => i.InterfaceSubtype);

                    var type = NetworkInterface.NetworkInterfaceType;
                    if (type == NetworkInterfaceType.None)
                    {
                        return;
                    }

                    using (DB3DataContext db = new DB3DataContext(DB3DataContext.DB3ConnectionString))
                    {
                        DB3 netData = new DB3();

                        netData = (from tarefa in db.DB3Items select tarefa).First();

                        if (netData.Type == 0 && !currentList.Contains(NetworkInterfaceSubType.WiFi))
                        {
                            return;
                        }
                    }

                    try
                    {
                        string file_path = "";

                        using (DB2DataContext db = new DB2DataContext(DB2DataContext.DB2ConnectionString))
                        {
                            List<DB2> list = (from tarefa in db.DB2Items select tarefa).ToList();
                            for (int i = 0; i < list.Count; i++)
                            {
                                await Task.Delay(TimeSpan.FromSeconds(1));

                                dados = list.ElementAt(i);
                                byte[] device = (byte[])DeviceExtendedProperties.GetValue("DeviceUniqueId");
                                string deviceString = BitConverter.ToString(device);

                                try
                                {
                                    IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();
                                    
                                    string errorString = string.Empty;
                                    TodoItem todoItem = new TodoItem();

                                    // Set blob properties of TodoItem.
                                    todoItem.ContainerName = "notafiscal" + Guid.NewGuid().ToString();

                                    // Use a unigue GUID to avoid collisions.
                                    todoItem.ResourceName = Guid.NewGuid().ToString();
                                    string resource = todoItem.ResourceName;

                                    // Send the item to be inserted. When blob properties are set this
                                    // generates an SAS in the response.
                                    await todoTable.InsertAsync(todoItem);

                                    // If we have a returned SAS, then upload the blob.
                                    if (!string.IsNullOrEmpty(todoItem.SasQueryString))
                                    {
                                        // Get the URI generated that contains the SAS 
                                        // and extract the storage credentials.
                                        StorageCredentials cred = new StorageCredentials(todoItem.SasQueryString);
                                        var imageUri = new Uri(todoItem.ImageUri);

                                        // Instantiate a Blob store container based on the info in the returned item.
                                        CloudBlobContainer container = new CloudBlobContainer(
                                            new Uri(string.Format("https://{0}/{1}",
                                                imageUri.Host, todoItem.ContainerName)), cred);


                                        // Upload the new image as a BLOB from the stream.

                                        CloudBlockBlob blobFromSASCredential = container.GetBlockBlobReference(resource);


                                        await blobFromSASCredential.UploadFromByteArrayAsync(dados.File, 0, dados.File.Length);

                                        file_path = blobFromSASCredential.Uri.ToString();

                                    }

                                    IMobileServiceTable<images_data> table = App.MobileService.GetTable<images_data>();
                                    images_data image_data = new images_data();

                                    JObject jo = new JObject();
                                    jo.Add("institution_id", dados.Institution_Id);
                                    jo.Add("platform", "WINDOWS");
                                    jo.Add("identificator", deviceString);
                                    jo.Add("status", "NOT_PROCESSED");
                                    jo.Add("date", "aa");
                                    jo.Add("file_path", file_path);


                                    var inserted = await table.InsertAsync(jo);

                                    using (DB2DataContext db2 = new DB2DataContext(DB2DataContext.DB2ConnectionString))
                                    {
                                        db2.DB2Items.Attach(dados);
                                        db2.DB2Items.DeleteOnSubmit(dados);
                                        db2.SubmitChanges();
                                    }
                                    
                                }
                                catch (Exception e)
                                {
                                }
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
                catch (Exception ex)
                {
                }

            }

            }

    
    }
}
