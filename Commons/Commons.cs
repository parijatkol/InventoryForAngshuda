using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;


namespace inventory_control
{
    class Commons
    {
        SqlDataAdapter MyDataAdapter = new SqlDataAdapter();

        clsGlobalValue MyValues = new clsGlobalValue();
        
        public void populateBrandMaster(DataAccessLayer MyDataLayer, DataTable myDataTable)
        {
            string strSQL = "select '' AS BrandID,'' AS BrandCode,'' AS BrandName from tbl_BrandMaster where Status=1 union "; 
                   strSQL += "select BrandID,BrandCode,BrandName from tbl_BrandMaster where Status=1";
            
            MyDataAdapter = MyDataLayer.PopulateData(strSQL);
            MyDataAdapter.Fill(myDataTable);
        }

        public void populateItemGroupMaster(DataAccessLayer MyDataLayer, DataTable myDataTable)
        {
            string strSQL = "select -1 AS ItemGroupCode,'' AS ItemGroupName from tbl_ItemGroupMaster where Status=1 union "; 
                   strSQL += "select ItemGroupCode,ItemGroupName from tbl_ItemGroupMaster where Status=1";
            MyDataAdapter = MyDataLayer.PopulateData(strSQL);
            MyDataAdapter.Fill(myDataTable);
        }

        public void populateUnitMaster(DataAccessLayer MyDataLayer, DataTable myDataTable)
        {
            string strSQL = "select -1 AS UnitID,'' AS UnitName from tbl_UnitMaster where Status=1 union "; 
                   strSQL += "select UnitID,UnitName from tbl_UnitMaster where Status=1";
            MyDataAdapter = MyDataLayer.PopulateData(strSQL);
            MyDataAdapter.Fill(myDataTable);
        }

        public void populateSupplierMaster(DataAccessLayer MyDataLayer, DataTable myDataTable)
        {
            string strSQL = "select -1 AS SupplierID,'' AS SupplierName from tbl_Suppliers where Status=1 union ";
            strSQL += "select SupplierID,SupplierName from tbl_Suppliers where Status=1";
            MyDataAdapter = MyDataLayer.PopulateData(strSQL);
            MyDataAdapter.Fill(myDataTable);
        }

        public void populateCustomerMaster(DataAccessLayer MyDataLayer, DataTable myDataTable)
        {
            string strSQL = "select -1 AS CustomerID,'' AS CustomerName from tbl_Customers where Status=1 union ";
            strSQL += "select CustomerID,CustomerName from tbl_Customers where Status=1";
            MyDataAdapter = MyDataLayer.PopulateData(strSQL);
            MyDataAdapter.Fill(myDataTable);
        }

        public DataTable populateItemMasterForGrid(DataAccessLayer MyDataLayer, string tblLookUp)
        {
            DataSet ds = new DataSet();
            string strSQL = "select -1 AS ItemNo,'' as ItemCode,'' AS ItemName,0 as Amount from tbl_ItemMaster where Status=1 union ";
            strSQL += "select ItemNo, ItemCode, ItemName, UnitPrice as Amount from tbl_ItemMaster where Status=1";
            MyDataAdapter = MyDataLayer.PopulateData(strSQL);
            MyDataAdapter.Fill(ds, tblLookUp);
            return ds.Tables[tblLookUp];
        }

        public DataSet populatePurchaseMasterDetailsRelationInGrid(int rec, DataAccessLayer MyDataLayer, string tblGrid)
        {
            DataSet ds = new DataSet();
            //string strSQL = "select b.ItemCode,b.ItemName,Amount,Qty,a.ItemNo,b.UnitName from tbl_PurchaseDetail a ";
              //     strSQL += "inner join tbl_PurchaseMaster c on c.PurchaseID = a.PurchaseID ";
              //      strSQL += "inner join tbl_ItemMaster b on a.ItemNo = b.ItemNo where a.PurchaseID = " + rec + " and a.Status=1 and c.FinancialYrID =" + MyValues.FinYearID;

            string strSQL = "select b.ItemCode,b.ItemName,Amount,Qty,a.ItemNo,d.UnitName from tbl_PurchaseDetail a ";
                 strSQL += "inner join tbl_PurchaseMaster c on c.PurchaseID = a.PurchaseID ";
                 strSQL += "inner join tbl_ItemMaster b on a.ItemNo = b.ItemNo ";
                 strSQL += "inner join tbl_Unitmaster d on d.UnitID = b.UnitID where a.PurchaseID = " + rec + " and a.Status=1 and c.FinancialYrID =" + MyValues.FinYearID;

            
            MyDataAdapter = MyDataLayer.PopulateData(strSQL);
            MyDataAdapter.Fill(ds, tblGrid);
            return ds;
        }

        public void populatePurchaseMaster(DataAccessLayer MyDataLayer, DataTable myDataTable, string invno,string suppid)
        {
            string strSQL ;
            if (invno !="")            
                strSQL = "select PurchaseID,InvoiceNo, CONVERT(char(10), InvoiceDate, 105) as InvoiceDate from tbl_PurchaseMaster where Status=1 and FinancialYrID =" + MyValues.FinYearID + " and InvoiceNo='" + invno + "'" ;
            else if(suppid !="" && Convert.ToInt16(suppid)>0)
                strSQL = "select PurchaseID,InvoiceNo, CONVERT(char(10), InvoiceDate, 105) as InvoiceDate from tbl_PurchaseMaster where Status=1 and FinancialYrID =" + MyValues.FinYearID + " and SupplierID=" + suppid;            
            else
                strSQL = "select PurchaseID,InvoiceNo, CONVERT(char(10), InvoiceDate, 105) as InvoiceDate from tbl_PurchaseMaster where Status=1 and FinancialYrID =" + MyValues.FinYearID;

            MyDataAdapter.Dispose();
            MyDataAdapter = MyDataLayer.PopulateData(strSQL);
            MyDataAdapter.Fill(myDataTable);
        }

        public DataSet populateIssueMasterDetailsRelationInGrid(int rec, DataAccessLayer MyDataLayer, string tblGrid)
        {
            DataSet ds = new DataSet();
            //string strSQL = "select b.ItemCode,b.ItemName,Amount,Qty,a.ItemNo,b.UnitName from tbl_PurchaseDetail a ";
            //     strSQL += "inner join tbl_PurchaseMaster c on c.PurchaseID = a.PurchaseID ";
            //      strSQL += "inner join tbl_ItemMaster b on a.ItemNo = b.ItemNo where a.PurchaseID = " + rec + " and a.Status=1 and c.FinancialYrID =" + MyValues.FinYearID;

            string strSQL = "select b.ItemCode,b.ItemName,Amount,Qty,a.ItemNo,d.UnitName from tbl_IssueDetails a ";
            strSQL += "inner join tbl_IssueMaster c on c.IssueID = a.IssueID ";
            strSQL += "inner join tbl_ItemMaster b on a.ItemNo = b.ItemNo ";
            strSQL += "inner join tbl_Unitmaster d on d.UnitID = b.UnitID where a.IssueID = " + rec + " and a.Status=1 and c.FinancialYrID =" + MyValues.FinYearID;


            MyDataAdapter = MyDataLayer.PopulateData(strSQL);
            MyDataAdapter.Fill(ds, tblGrid);
            return ds;
        }
        public void populateIssueMaster(DataAccessLayer MyDataLayer, DataTable myDataTable, string invno, string custid)
        {
            string strSQL;
            if (invno != "")
                strSQL = "select IssueID,InvoiceNo, CONVERT(char(10), InvoiceDate, 105) as InvoiceDate from tbl_IssueMaster where Status=1 and FinancialYrID =" + MyValues.FinYearID + " and InvoiceNo='" + invno + "'";
            else if (custid != "" && Convert.ToInt16(custid) > 0)
                strSQL = "select IssueID,InvoiceNo, CONVERT(char(10), InvoiceDate, 105) as InvoiceDate from tbl_IssueMaster where Status=1 and FinancialYrID =" + MyValues.FinYearID + " and SupplierID=" + custid;
            else
                strSQL = "select IssueID,InvoiceNo, CONVERT(char(10), InvoiceDate, 105) as InvoiceDate from tbl_IssueMaster where Status=1 and FinancialYrID =" + MyValues.FinYearID;

            MyDataAdapter.Dispose();
            MyDataAdapter = MyDataLayer.PopulateData(strSQL);
            MyDataAdapter.Fill(myDataTable);
        }

        public void ChangeCurrencyFormat(string symbol)
        {
            CultureInfo culture = new CultureInfo("en-US", true);
            culture.NumberFormat.CurrencySymbol = " " + symbol;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            DevExpress.Utils.FormatInfo.AlwaysUseThreadFormat = true;
        }


        public decimal CalculateTotalAmt(string invTotal,string[] Params1,string[] Params2)
        {
            decimal Total = 0;
            decimal val = 0;

            Total = Convert.ToDecimal(invTotal);
            for (int i = 0; i < Params1.Length;i++)
            {
                if (Params1[i] == "")
                    val = 0;
                else
                    val = Convert.ToDecimal(Params1[i]);

                if (Params2[i] == "+")
                    Total = Total + val;
                else if (Params2[i] == "-")
                    Total = Total - val;
            }
            return Total;    
        }

        public decimal ToRoundedAmount(decimal val)
        {
            decimal fractionalValue = val - Decimal.Truncate(val);            

/*            if (fractionalValue < 50)
                val = Convert.ToInt32(val);
            else
                val = Convert.ToInt32(val) + 1; */

            return fractionalValue;
        }


        public bool  CheckGridItem(GridView view,string column1,string column2)
        {
            bool flag = false;

            for (int k = 0; k < view.RowCount; k++)
            {
                DataRow row = view.GetDataRow(k);

                if (row[column1].ToString() == "" && row[column2].ToString() == "")
                {
                    flag = false;
                    break;
                }
                else
                //{
                   // int issueQty = (int)row[column2];
                    flag = true;
                //}
            }

            return flag;
        }

        public string checkItemOrderLevel(int itemno, int issueQty)
        {
            SqlDataReader rdr;

            //bool flag = false;
            int stockInHand = 0, reorderqty = 0;
            DataAccessLayer InvDataAccessLayer = new DataAccessLayer();

            string strSQL = "select ItemCode, ItemName, IsNull(ReOrderLevel,0) as ReOrderLevel, ClosingStock from tbl_ItemMaster IM ";
            strSQL += "inner join tbl_StockMaster SM on (IM.ItemNo = SM.ItemNo)  where IM.Status=1 and IM.ItemNo =" + itemno;

            rdr = InvDataAccessLayer.GetDataReaderSql(strSQL);

            while (rdr.Read())
            {
                reorderqty = (int)rdr["ReOrderLevel"];
                stockInHand = (int)rdr["ClosingStock"];  // CS = 10   ISS = 6  RL = 5

                //if ((stockInHand - issueQty) < reorderqty)
                //    flag = false;
                //else
                //    flag = true;
            }
            //return flag;
            return reorderqty.ToString() + "," + stockInHand.ToString();
        }

        public void SaleInvoiceHTML(string invno)
        {
            string strSQL = string.Empty;
            string strIssueID = string.Empty;
            string strCustomerName = string.Empty;
            decimal totalamt = 0, delvcharges = 0, discount = 0, vatamt = 0, netamt=0,row1Amt = 0;
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataAccessLayer InvDataAccessLayer = new DataAccessLayer();
            StreamWriter sFile = new StreamWriter(Application.StartupPath + "/list.html", false, System.Text.Encoding.UTF8); 

            if (invno != "")
            {
                strSQL = "select a.IssueID, a.CustomerID, b.CustomerName ,a.TotalAmt,a.VatAmt,a.Adjustments,isnull(a.deliverycharges,0) as deliverycharges,isnull(a.discount,0) as discount,a.RoundedAmt,a.Remarks,a.InvoiceNo, ";
                strSQL += "a.InvoiceDate,b.Address1,b.Phone from tbl_IssueMaster a inner join ";
                strSQL += "tbl_Customers b on a.CustomerID= b.CustomerID where a.Status=1 and a.FinancialYrID = " + MyValues.FinYearID + " and a.InvoiceNo='" + invno.Trim() + "' and a.Status=1 and b.Status=1";

                ds = InvDataAccessLayer.PopulateDataSet(strSQL, "IssueMaster");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        //            _CustomerID = Convert.ToInt16(row["CustomerID"]);
                        //            _CustomerIDItemIndex = Convert.ToInt16(row["CustomerIDItemIndex"]);
                        //            _InvoiceNo = row["InvoiceNo"].ToString();
                        //            _InvoiceDate =Convert.ToDateTime(row["InvoiceDate"]);
                        //            _Remarks = row["Remarks"].ToString();
                        //            _Adjustments = Convert.ToDecimal(row["Adjustments"].ToString());
                        //            _deliverycharges = Convert.ToDecimal(row["deliverycharges"].ToString());
                        //            _discount = Convert.ToDecimal(row["discount"].ToString());
                        //            _VatAmt = Convert.ToDecimal(row["VatAmt"]);
                        //            _RoundedAmt = Convert.ToDecimal(row["RoundedAmt"]);
                        //            _Address = row["Address1"].ToString();
                        //            _phone = row["Phone"].ToString();
                        ////            _CSTNo = row["CST"].ToString();
                        ////            _VATNo = row["VAT"].ToString();
                        //            _TotalAmt =Convert.ToDecimal(row["TotalAmt"]);

                        strCustomerName = row["CustomerName"].ToString();
                        strIssueID = row["IssueID"].ToString();
                        delvcharges = Convert.ToDecimal(row["deliverycharges"]);
                        discount = Convert.ToDecimal(row["discount"]);
                        vatamt = Convert.ToDecimal(row["VatAmt"]);
                        totalamt = Convert.ToDecimal(row["TotalAmt"]);
                        netamt = totalamt - discount;


                        sFile.Write(@"<!DOCTYPE html>" + sFile.NewLine + "<html lang='en'>" + sFile.NewLine);
						sFile.Write(@"<head><meta charset='UTF-8'>" + sFile.NewLine + "<title>Invoice</title>" + sFile.NewLine);
                        sFile.Write(@"<link rel='stylesheet' href='css/style.css'>" + sFile.NewLine);
                        sFile.Write(@"<link rel='stylesheet' href='css/print.css' media='print'>" + sFile.NewLine);
                        sFile.Write(@"<script type = 'text/javascript' src='js/main.js'></script" + sFile.NewLine);						
                        sFile.Write(@"</head>" + sFile.NewLine);
                        sFile.Write(@"<body>" + sFile.NewLine);
                        sFile.Write(@"<div id='page-wrap'>" + sFile.NewLine);
						sFile.Write(@"<div style='margin:0 auto; width:300px;'><h2>ABC Company Pvt. Ltd</h2></div>" + sFile.NewLine);
						sFile.Write(@"<div id='header'>Sale &nbsp;Invoice</div>" + sFile.NewLine + "<div id='identity'></div>" + sFile.NewLine + "<div style='clear:both'></div>" + sFile.NewLine);
						sFile.Write(@"<div id='customer'><div style='float:left; width: 40%; margin-top:5px; font-size:12px;' id='customer-title'>");
                        sFile.Write(@"To,<br>" +  strCustomerName + "<br>" + row["Address1"].ToString() + "</div>" + sFile.NewLine);
                        sFile.Write(@"<table id='meta'>" + sFile.NewLine + "<tr>" + sFile.NewLine + "<td class='meta-head'>Invoice #</td><td>" + invno.ToString() + "</td></tr>" + sFile.NewLine + "<tr>" + sFile.NewLine + "<td class='meta-head'>Date</td><td>" + String.Format("{0:d/M/yyyy}", Convert.ToDateTime(row["InvoiceDate"])) + "</td></tr>" + sFile.NewLine);
                        sFile.Write(@"<tr>" + sFile.NewLine + "<td class='meta-head'>Net Amount (Rs) </td><td><div class='due'>" + netamt + "</div></td></tr>" + sFile.NewLine + "</table>" + sFile.NewLine + "</div>");
                    }
                }

                sFile.Write(@"<table id='items'>" + sFile.NewLine + "<tr>" + sFile.NewLine + "<th>Sl#</th><th>Item</th><th>Description</th><th>Unit Cost</th><th>Quantity</th><th>Price</th></tr>");

                strSQL = "select b.ItemCode,b.ItemName,Amount,Qty,a.ItemNo,d.UnitName from tbl_IssueDetails a ";
                strSQL += "inner join tbl_IssueMaster c on c.IssueID = a.IssueID ";
                strSQL += "inner join tbl_ItemMaster b on a.ItemNo = b.ItemNo ";
                strSQL += "inner join tbl_Unitmaster d on d.UnitID = b.UnitID where a.IssueID = " + strIssueID + " and a.Status=1 and c.FinancialYrID =" + MyValues.FinYearID;
                ds1 = InvDataAccessLayer.PopulateDataSet(strSQL, "IssueDetails");


                if (ds1.Tables[0].Rows.Count > 0)
                {
					
                    int cnt = 1;
                    foreach (DataRow row1 in ds1.Tables[0].Rows)
					{
                       sFile.Write(@"<tr class='item-row'><td class='sl'>" +  cnt + "</td><td class='item-name'>" + row1["ItemCode"].ToString() + "</td><td class='description'>" + row1["ItemName"].ToString() + "</td><td class='price'>" + Convert.ToDecimal(row1["Amount"])  + "</td><td class='qty'>" + row1["Qty"].ToString() + "</td><td class='price'>" + Convert.ToDecimal(row1["Qty"]) * Convert.ToDecimal(row1["Amount"]) + "</td></tr>");
					   row1Amt +=  Convert.ToDecimal(row1["Qty"]) * Convert.ToDecimal(row1["Amount"]);
                       cnt++; 
                    }  
                }
                ds1.Dispose();

                sFile.Write(@"<tr><td colspan='3' class='blankSubTotal'> </td><td colspan='2' class='total-line'><b>Subtotal (Rs.)</b></td><td class='price'>" +   row1Amt + "</td></tr>");
                sFile.Write(@"<tr><td colspan='3' class='blank'> </td><td colspan='2' class='total-line'>Delivery Charge (Rs.)</td><td class='price'>" + ((delvcharges > 0) ? delvcharges.ToString() : "") + "</td></tr>");
                sFile.Write(@"<tr><td colspan='3' class='blank'> </td><td colspan='2' class='total-line'>Vat Amt (Rs.)</td><td class='price'>" + ((vatamt > 0 ) ? vatamt.ToString() : "")+ "</td></tr>");
                sFile.Write(@"<tr><td colspan='3' class='blank'> </td><td colspan='2' class='total-line'><b>Total Amt (Rs.)</b></td><td class='price'>" + Convert.ToDecimal(totalamt) + "</td></tr>");
                sFile.Write(@"<tr><td colspan='3' class='blank'> </td><td colspan='2' class='total-line'>Discount (Rs.)</td><td class='price'>" + ((discount > 0) ? discount.ToString() : "")+ "</td></tr>");
                sFile.Write(@"<tr><td colspan='3'>Total (In Words) Rs. <em>Sum of rupees............</em></td><td colspan='2' class='total-line balance'>Net Total</td><td class='price'>" +  netamt + "</td></tr></table>");
                sFile.Write(@"</div></body></html>");

                sFile.Flush();
                sFile.Close();
                ds.Dispose();

                System.Diagnostics.Process.Start(Application.StartupPath + "/list.html");
            }
            
        }

    }

    public class PersonInfo
    {
        private string _firstName;
        private string _lastName;

        public PersonInfo(string firstName, string lastName)
        {
            _firstName = firstName;
            _lastName = lastName;
        }

        public override string ToString()
        {
            return _firstName + " " + _lastName;
        }
    }
}
