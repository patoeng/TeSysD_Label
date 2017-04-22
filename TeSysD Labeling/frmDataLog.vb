Imports MySql.Data.MySqlClient

Public Class frmDataLog

    Private Sub frmDataLog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        viewListDataLog(lblSort.Text)
    End Sub

    Public Sub viewListDataLog(ByVal sortVal As String)
        txtPass.Text = viwSOQty("pass_qty", sortVal)
        txtFail.Text = viwSOQty("fail_qty", sortVal)


        On Error Resume Next
        dgvDataLog.Rows.Clear()
        dgvDataLog.Columns.Clear()
        Dim strSQL As String
        Call connectServer()
        If lblSort.Text = "Reference" Then
            strSQL = "SELECT prd_order, prd_reference, wip, st2_check, st4_check, final_result, start_time, end_time FROM tblLogTester WHERE start_time BETWEEN '" & dtpStartDate.Text & "' AND '" & dtpEndDate.Text & "'"
        ElseIf lblSort.Text = "Prd. Ord. #" Then
            strSQL = "SELECT prd_order, prd_reference, wip, st2_check, st4_check, final_result, start_time, end_time FROM tblLogTester WHERE prd_order = '" & sortVal & "'"
        Else
            strSQL = "SELECT prd_order, prd_reference, wip, st2_check, st4_check, final_result, start_time, end_time FROM tblLogTester"
        End If
        com = New MySqlCommand(strSQL, conn)
        Dim da As New MySqlDataAdapter(strSQL, conn)
        Dim ds As New DataSet

        da.Fill(ds)
        da.Dispose()

        dgvDataLog.Columns.Add("Prd. Ord. #", "Prd. Ord. #")
        dgvDataLog.Columns.Add("Reference Order", "Reference Order")
        dgvDataLog.Columns.Add("WIP", "WIP")
        dgvDataLog.Columns.Add("ST2 Check", "ST2 Check")
        dgvDataLog.Columns.Add("ST4 Check", "ST4 Check")
        dgvDataLog.Columns.Add("Final Result", "Final Result")

        dgvDataLog.Columns.Add("Start Time", "Start Time")
        dgvDataLog.Columns.Add("End Time", "End Time")

        Dim baris = ds.Tables(0).Rows


        For part = 0 To baris.Count - 1

            dgvDataLog.Rows.Add(baris(part).Item("prd_order").ToString, baris(part).Item("prd_reference").ToString, baris(part).Item("wip").ToString, baris(part).Item("st2_check").ToString, baris(part).Item("st4_check").ToString, baris(part).Item("final_result").ToString, baris(part).Item("start_time").ToString, baris(part).Item("end_time").ToString)

        Next

    End Sub

    Function viwSOClose(ByVal field As String, ByVal so As String) As String
        On Error Resume Next
        'If checkWO(so) = True Then
        Call connectServer()
        Dim strSQL = "SELECT prd_order, prd_reference, prd_qty, pass_qty, fail_qty FROM tblWorkOrder WHERE prd_order = '" & so & "' AND flag = 0"
        com = New MySqlCommand(strSQL, conn)
        Dim reader As MySqlDataReader = com.ExecuteReader

        reader.Read()

        If reader.HasRows Then
            Return reader(field).ToString
        Else
            Return "Need to Complete Prd. Order!"
        End If
        'End If
    End Function

    Function viwSOQty(ByVal field As String, ByVal so As String) As String
        On Error Resume Next
        'If checkWO(so) = True Then
        Call connectServer()
        Dim strSQL = "SELECT prd_order, prd_reference, prd_qty, pass_qty, fail_qty FROM tblWorkOrder WHERE prd_order = '" & so & "'"
        com = New MySqlCommand(strSQL, conn)
        Dim reader As MySqlDataReader = com.ExecuteReader

        reader.Read()

        If reader.HasRows Then
            Return reader(field).ToString
        Else
            Return "Need to Complete Prd. Order!"
        End If
        'End If
    End Function

    Function checkFPY(ByVal valSONum As String) As String
        On Error Resume Next
        Dim FPQ As Integer = viwSOClose("pass_qty", valSONum).ToString
        Dim FBQ As Integer = viwSOClose("fail_qty", valSONum).ToString
        Dim FPY
        If FPQ <> "Need to Complete Prd. Order!" Then
            FPY = FPQ / (FPQ + FBQ)
            Return FPY * 100
        Else
            Return FPQ
        End If
    End Function

    Sub sortDataLog()
        If txtSortir.Text = "" Then
            viewListDataLog("*")
        Else
            viewListDataLog(txtSortir.Text)
            txtFPY.Text = checkFPY(txtSortir.Text)
            If checkFPY(txtSortir.Text) <> "Need to Complete Prd. Order!" Then
                dgvDataLog.Rows.Add("     ", "     ", "     ", "     ", "     ", "     ", "     ")
                dgvDataLog.Rows.Add("     ", "     ", "     ", "     ", "     ", "     ", "     ")
                dgvDataLog.Rows.Add("     ", "     ", "     ", "     ", "     ", "FPY QTY : ", checkFPY(txtSortir.Text))
            End If
        End If

    End Sub

    Private Sub btnSort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSort.Click
        'MsgBox(frmMain.txtSONumber.Text)
        sortDataLog()
    End Sub

    Sub saveToExcel(ByVal valPath As String) ' ByVal valExt As String)

        Dim StrExport As String = ""
        For Each C As DataGridViewColumn In dgvDataLog.Columns
            StrExport &= " " & C.HeaderText & ","
        Next
        StrExport = StrExport.Substring(0, StrExport.Length - 1)
        StrExport &= Environment.NewLine

        For Each R As DataGridViewRow In dgvDataLog.Rows
            For Each C As DataGridViewCell In R.Cells
                If Not C.Value Is Nothing Then
                    StrExport &= " " & C.Value.ToString & ","
                Else
                    'StrExport &= "," & ","
                End If
            Next
            StrExport = StrExport.Substring(0, StrExport.Length - 1)
            StrExport &= Environment.NewLine
        Next

        Dim BDATENOW1 As String

        BDATENOW1 = valPath

        Dim tw As IO.TextWriter = New IO.StreamWriter(BDATENOW1)
        tw.Write(StrExport)
        tw.Close()

        MsgBox("Data Log has been download!", MsgBoxStyle.Information)

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If dgvDataLog.RowCount = 0 Then
            MsgBox("No data recorded for this " & lblSort.Text, MsgBoxStyle.Information)
        Else

            'ofdSaveLog.ShowDialog()
            sfdLogExcel.FileName = "TeSys D Test Log " & Replace(frmMain.dateNowMysql, ":", ".")
            sfdLogExcel.AddExtension = True
            sfdLogExcel.DefaultExt = ".CSV"

            If sfdLogExcel.ShowDialog = DialogResult.OK Then
                saveToExcel(sfdLogExcel.FileName)
            End If
        End If

    End Sub

    Private Sub chkSort_ChecmkedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSort.CheckedChanged
        If chkSort.Checked = True Then
            lblSort.Text = "Reference"
            dtpStartDate.Enabled = True
            dtpEndDate.Enabled = True
        Else
            lblSort.Text = "Prd. Ord. #"
            dtpStartDate.Enabled = False
            dtpEndDate.Enabled = False
        End If
    End Sub

    Private Sub sfdLogExcel_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles sfdLogExcel.FileOk
        saveToExcel(sfdLogExcel.FileName) ', sfdLogExcel.DefaultExt)
    End Sub

    Private Sub txtFPY_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If txtFPY.Text <> "" Then
            txtFPY.Text = txtFPY.Text
        End If
    End Sub

    Private Sub pnlMain_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlMain.Paint
        dgvDataLog.Rows.Clear()
        dgvDataLog.Columns.Clear()

        dgvDataLog.Columns.Add("Prd. Ord. #", "Prd. Ord. #")
        dgvDataLog.Columns.Add("Reference Order", "Reference Order")
        dgvDataLog.Columns.Add("WIP", "WIP")
        dgvDataLog.Columns.Add("ST2 Check", "ST2 Check")
        dgvDataLog.Columns.Add("ST4 Check", "ST4 Check")
        dgvDataLog.Columns.Add("Final Result", "Final Result")

        dgvDataLog.Columns.Add("Loged By", "Loged By")

    End Sub

    Private Sub txtSortir_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSortir.KeyPress
        If e.KeyChar = vbCr Then
            sortDataLog()
        End If
    End Sub
End Class