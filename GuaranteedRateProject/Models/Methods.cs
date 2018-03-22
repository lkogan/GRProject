using GuaranteedRateProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuaranteedRateProject.Helpers
{
    class Methods
    { 
        public Result IsValidFile(string FilePath, string allowedExtension)
        {
            if (!File.Exists(FilePath))
            {
                return new Result { Successful = false, Information = "Error: File does not exist at: " + FilePath };
            }

            if (!IsExtensionAllowed(FilePath, allowedExtension))
            {
                string fileExt = Path.GetExtension(FilePath);
                fileExt = CleanExtension(fileExt);

                return new Result { Successful = false, Information = string.Format("{0} is not an allowed file extension. Required: {1}", fileExt, allowedExtension) };
            }

            if (!IsReadable(FilePath))
            {
                return new Result { Successful = false, Information = "File could not be read at: " + FilePath };
            }

            return new Result { Successful = true, Information = string.Empty };
        }

        public Result FileDataPopulated(string FilePath)
        {  
            Result result = new Result { Successful = true, Information = string.Empty };
            StringBuilder sb = new StringBuilder();

            foreach (string line in File.ReadLines(FilePath))
            {
                bool linePopulated = LinePopulated(line);

                if (!linePopulated)
                {
                    result.Successful = false;
                    sb.Append("\n");
                    sb.Append(string.Format("Could not process line: {0}.", line));
                    result.Information = sb.ToString();
                }
            }

            return result;
        }

        public bool LinePopulated(string FileLine)
        {
            try
            {
                //Data in the files, per specs, contains spaces along with these delimiters
                if (FileLine.Contains(" | ") || FileLine.Contains(", "))
                {
                    FileLine = FileLine.Replace(" ", string.Empty);
                }

                List<string> delimiters = new List<string>();
                delimiters.Add("|");
                delimiters.Add(",");
                delimiters.Add(" ");

                for (int i = 0; i < delimiters.Count; i++)
                {
                    if (FileLine.Contains(delimiters[i]))
                    {
                        var parts = FileLine.Split(new[] { delimiters[i] }, StringSplitOptions.None);

                        if (parts.Count() != 5)
                        {
                            return false;
                        }
                        else
                        { 
                            //Suggested checks I would add:
                                //Check if data pieces are not swapped/missed
                                //Check if gender is M/F only
                                //Check if name is alpha name
                                //Check if date is last, valid format (US)

                            Person p = new Person();
                            p.LastName = parts[0];
                            p.FirstName = parts[1];
                            p.Gender = parts[2];
                            p.FavoriteColor = parts[3];
                            p.DateOfBirth = DateTime.Parse(parts[4]);

                            Model.ListOfPersons.Add(p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public string CleanExtension(string ext)
        {
            return ext.ToLower().Replace(".", string.Empty);
        }

        public bool IsReadable(string FilePath)
        {
            bool canRead = true;
            try
            {
                using (FileStream stream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
                {
       
                }
            }
            catch (Exception)
            {
                return false;
            }

            return canRead;
        }

        public bool IsExtensionAllowed(string FilePath, string allowedExtension)
        {
            bool extAllowed = true;
             
            string ext = Path.GetExtension(FilePath);
            ext = CleanExtension(ext);

            allowedExtension = CleanExtension(allowedExtension);

            if (!ext.Equals(allowedExtension))
            {
                return false;
            } 

            return extAllowed;
        }

    }
}
