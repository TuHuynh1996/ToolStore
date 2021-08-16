using DSC_CreateDataTestTool.ExcelDTO;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DSC_CreateDataTestTool
{
    public class Service
    {
        MainModel mainModel;
        public Service(MainModel mainModel)
        {
            // If you are a commercial business and have
            // purchased commercial licenses use the static property
            // LicenseContext of the ExcelPackage class:
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            this.mainModel = mainModel;
        }
        public void DoProcess()
        {
            List<InputItemDTO> inputItemList = ReadAllItem(mainModel.ItemNameInput);
            List<OutputItemDTO> outputItemList = SetOutPut(inputItemList);
            if (!string.IsNullOrWhiteSpace(mainModel.ItemJsonInput))
            {
                AddJsonName(in outputItemList);
            }
            ExporeExcel(outputItemList);
        }

        private void AddJsonName(in List<OutputItemDTO> outputItemList)
        {

            FileInfo existingFile = new FileInfo(mainModel.ItemJsonInput);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[mainModel.ItemJsonSheet - 1];
                int colCount = worksheet.Dimension.End.Column;  //get Column Count
                int rowCount = worksheet.Dimension.End.Row;     //get row count

                foreach (OutputItemDTO outputItemDTO in outputItemList)
                {
                    for (int row = 8; row <= rowCount; row++)
                    {
                        if (!string.IsNullOrEmpty(worksheet.Cells[row, 11].Text) && outputItemDTO.Name.Replace("_", "").Equals(worksheet.Cells[row, 11].Text?.ToString().Trim().Replace("_", "")))
                        {
                            outputItemDTO.JsonName = worksheet.Cells[row, 12].Value?.ToString().Trim();
                            break;
                        }
                    }
                }
            }
        }

        private void ExporeExcel(List<OutputItemDTO> outputItemList)
        {
            File.Copy(@"./ExcelDTO/Output.xlsx", mainModel.Output + @"/Output.xlsx");
            FileInfo newFile = new FileInfo(mainModel.Output + @"/Output.xlsx");
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[0];
                for (int i = 0; i < outputItemList.Count; i++)
                {
                    worksheet.Cells[29 + i, 1].Value = i + 1;
                    worksheet.Cells[29 + i, 2].Value = outputItemList[i].Name;
                    worksheet.Cells[29 + i, 5].Value = outputItemList[i].Digit;
                    worksheet.Cells[29 + i, 6].Value = outputItemList[i].IOInfo;
                    worksheet.Cells[29 + i, 8].Formula = "= SUM($E$3:$E" + (28 + i) + ") + 1";
                    worksheet.Cells[29 + i, 9].Formula = "= SUM($E$3:$E" + (29 + i) + ")";
                    worksheet.Cells[29 + i, 12].Value = outputItemList[i].JsonName;
                }
                xlPackage.Save();
            }
        }

        private List<OutputItemDTO> SetOutPut(List<InputItemDTO> itemList)
        {
            List<OutputItemDTO> outputItemList = new List<OutputItemDTO>();
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].RepeatNumber > 0)
                {
                    List<InputItemDTO> itemListSub = new List<InputItemDTO>();
                    int addNumber = 0;
                    int deleteNumber = 1;
                    for (int j = 1; j <= itemList[i].RepeatNumber; j++)
                    {
                        var itemListClone = itemList[i].Clone();
                        itemListClone.RepeatNumber = 0;
                        itemListClone.Name = itemListClone.Name + j;
                        itemListSub.Add(itemListClone);
                        addNumber++;
                        for (int k = i + 1; k < itemList.Count; k++)
                        {
                            if (itemList[k].Rank > itemList[i].Rank)
                            {
                                itemListSub.Add(itemList[k].Clone());
                                addNumber++;
                                if (j == 1)
                                {
                                    deleteNumber++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    itemList.RemoveRange(i, deleteNumber);
                    itemList.InsertRange(i, itemListSub);
                    i--;
                }
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].Digit > 0)
                {
                    outputItemList.Add(new OutputItemDTO()
                    {
                        No = i,
                        Name = itemList[i].Name,
                        Attributes = "",
                        Digit = itemList[i].Digit,
                        IOInfo = itemList[i].IOInfo,
                    });

                }
            }
            return outputItemList;
        }

        private List<InputItemDTO> ReadAllItem(string FilePath)
        {
            List<InputItemDTO> itemList = new List<InputItemDTO>();
            FileInfo existingFile = new FileInfo(FilePath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[mainModel.ItemNameSheet - 1];
                int colCount = worksheet.Dimension.End.Column;  //get Column Count
                int rowCount = worksheet.Dimension.End.Row;     //get row count
                for (int row = 11; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Value == null)
                    {
                        continue;
                    }
                    string itemName = null; int itemRank = 0;
                    for (int col = 2; col <= 4; col++)
                    {
                        itemName = worksheet.Cells[row, col].Value?.ToString();
                        if (!string.IsNullOrWhiteSpace(itemName))
                        {
                            itemRank = col - 1;
                            if (itemRank == 3)
                            {
                                itemRank = itemRank + itemName.Count(Char.IsWhiteSpace);
                            }
                            break;
                        }
                    }

                    InputItemDTO itemDTO = new InputItemDTO()
                    {
                        No = int.Parse(worksheet.Cells[row, 1].Value?.ToString().Trim() ?? "0"),
                        Name = itemName.Trim(),
                        Rank = itemRank,
                        Type = worksheet.Cells[row, 5].Value?.ToString().Trim(),
                        Digit = int.Parse(worksheet.Cells[row, 6].Value?.ToString().Trim() ?? "0"),
                        RepeatNumber = int.Parse(worksheet.Cells[row, 7].Value?.ToString().Trim() ?? "0"),
                        Potision = int.Parse(worksheet.Cells[row, 8].Value?.ToString().Trim() ?? "0"),
                        ID = worksheet.Cells[row, 10].Value?.ToString().Trim(),
                        IOInfo = worksheet.Cells[row, 11].Value?.ToString().Trim()
                    };

                    itemList.Add(itemDTO);
                }
            }
            return itemList;
        }
    }
}
