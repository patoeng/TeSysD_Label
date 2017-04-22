Imports MySql.Data.MySqlClient
Imports System.Data.OleDb
Imports System.IO

Imports Excel = Microsoft.Office.Interop.Excel

Public Class frmReference

    Public sensor1 As Integer
    Public sensor2 As Integer
    Public sensor3 As Integer
    Public sensor4 As Integer
    Public sensor5 As Integer
    Public sensor6 As Integer
    Public sensor7 As Integer

    Public Sub doSetMode(ByVal vParamMode As String)
        btnEditReference.Enabled = True
        btnDeleteReference.Enabled = True
        Select Case vParamMode
            Case "READ"
                txtReference.ReadOnly = True
                txtArtNumber.ReadOnly = True
                txtLabelCode.ReadOnly = True
                cmbFamily.Enabled = False
                cmbACDC.Enabled = False

                nudJob.ReadOnly = True
                nudJob.Enabled = False

                txtJobNamec.ReadOnly = True

                txtReference.BackColor = SystemColors.Control
                txtArtNumber.BackColor = SystemColors.Control
                txtLabelCode.BackColor = SystemColors.Control
                cmbFamily.BackColor = SystemColors.Control
                cmbACDC.BackColor = SystemColors.Control

            Case "EDIT"
                txtReference.ReadOnly = True
                txtArtNumber.ReadOnly = False
                txtLabelCode.ReadOnly = False
                cmbFamily.Enabled = True
                cmbACDC.Enabled = True

                nudJob.ReadOnly = False
                nudJob.Enabled = True
                txtJobNamec.ReadOnly = False

                txtReference.BackColor = Color.White
                txtArtNumber.BackColor = Color.White
                txtLabelCode.BackColor = Color.White
                cmbFamily.BackColor = Color.White
                cmbACDC.BackColor = Color.White

            Case "ADD"
                txtReference.ReadOnly = False
                txtArtNumber.ReadOnly = False
                txtLabelCode.ReadOnly = False
                cmbFamily.Enabled = True
                cmbACDC.Enabled = True

                txtReference.BackColor = Color.White
                nudJob.ReadOnly = False
                nudJob.Enabled = True

                txtJobNamec.ReadOnly = False

                txtArtNumber.BackColor = Color.White
                txtLabelCode.BackColor = Color.White
                cmbFamily.BackColor = Color.White
                cmbACDC.BackColor = Color.White

                txtReference.Text = ""
                txtArtNumber.Text = ""
                txtLabelCode.Text = ""
                cmbFamily.Text = ""
                cmbACDC.Text = ""

                nudJob.Text = ""
                txtJobNamec.Text = ""

                btnEditReference.Enabled = False
                btnDeleteReference.Enabled = False
        End Select

    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditReference.Click
        If txtReference.Text <> "" Then
            saveMode = "UPDATE"
            doSetMode("EDIT")
        End If
    End Sub

    Private Sub cmbACDCC_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbACDC.KeyPress
        e.Handled = True
    End Sub

    Public Sub viewListReference(ByVal refeLike As String)
        dgvReference.Rows.Clear()
        dgvReference.Columns.Clear()
        Dim strSQL As String
        Call connectServer()

        If refeLike = "" Then
            strSQL = "SELECT * FROM tblReference"
        Else
            strSQL = "SELECT * FROM tblReference WHERE refe_name LIKE '" & refeLike & "%'"
        End If


        com = New MySqlCommand(strSQL, conn)
        Dim da As New MySqlDataAdapter(strSQL, conn)
        Dim ds As New DataSet
        'Dim reader As MySqlDataReader = com.ExecuteReader
        'Dim source As New BindingSource

        da.Fill(ds)
        da.Dispose()

        dgvReference.Columns.Add("Reference", "Reference")
        dgvReference.Columns.Add("Art Number", "Art Number")
        dgvReference.Columns.Add("Label Code", "Label Code")
        dgvReference.Columns.Add("Family", "Family")
        dgvReference.Columns.Add("ACDC", "ACDC")

        dgvReference.Columns.Add("Job ID", "Job ID")

        dgvReference.Columns.Add("Created Date", "Created Date")
        dgvReference.Columns.Add("Created By", "Created By")
        dgvReference.Columns.Add("Modified Date", "Modified Date")
        dgvReference.Columns.Add("Modified By", "Modified By")

        Dim baris = ds.Tables(0).Rows

        For part = 0 To baris.Count - 1

            dgvReference.Rows.Add(baris(part).Item("refe_name").ToString, baris(part).Item("art_number").ToString, baris(part).Item("label_code").ToString, baris(part).Item("family").ToString, baris(part).Item("acdc").ToString, baris(part).Item("job_vision_id").ToString, baris(part).Item("created_date").ToString, baris(part).Item("created_by").ToString, baris(part).Item("modified_date").ToString, baris(part).Item("modified_by").ToString)

        Next

    End Sub

    Sub updateReference(ByVal refeName As String, ByVal artNumber As String, ByVal labelCode As String, ByVal family As String, ByVal acdc As String, ByVal jobID As Integer)
        Try
            Call connectServer()
            Dim strSQL = "UPDATE tblReference SET label_code = '" & labelCode & "', family = '" & family & "', acdc = '" & acdc & "', job_vision_id = '" & jobID & "', modified_date = '" & frmMain.dateNowMysql & "', modified_by = '" & frmMain.getUserName & "' WHERE refe_name = '" & refeName & "' "

            com = New MySqlCommand(strSQL, conn)
            com.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox("ERROR 2 : " & ex.Message)
        End Try

    End Sub

    Sub deleteReference()
        Try

            Call connectServer()
            Dim strSQL = "DELETE FROM tblReference WHERE refe_name = '" & txtReference.Text & "' "

            com = New MySqlCommand(strSQL, conn)
            com.ExecuteNonQuery()

            MsgBox("Delete Reference Done!", MsgBoxStyle.Information)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveReference.Click
        If saveMode = "INSERT" Then
            If txtJobNamec.Text <> "NOT FOUND!" Then
                insertReference(txtReference.Text, txtArtNumber.Text, txtLabelCode.Text, cmbFamily.Text, cmbACDC.Text, nudJob.Value)
                viewListReference("")
            Else
                MsgBox("Vision ID NOT FOUND!", MsgBoxStyle.Critical)
            End If
        ElseIf saveMode = "UPDATE" Then
            If txtJobNamec.Text <> "NOT FOUND!" Then
                updateReference(txtReference.Text, txtArtNumber.Text, txtLabelCode.Text, cmbFamily.Text, cmbACDC.Text, nudJob.Value)
                viewListReference(txtViewReference.Text)
            Else
                MsgBox("Vision ID NOT FOUND!", MsgBoxStyle.Critical)
            End If
        End If
        doSetMode("READ")
    End Sub

    Public Sub insertReference(ByVal refeName As String, ByVal artnumber As String, ByVal labelCode As String, ByVal family As String, ByVal acdc As String, ByVal jobID As String)
        Try


            Call connectServer()
            Dim strSQL = "INSERT INTO tblReference (refe_name, art_number, label_code, family, acdc, job_vision_id, created_date, created_by) VALUES ('" & refeName & "', '" & artnumber & "', '" & labelCode & "', '" & family & "', '" & acdc & "', '" & jobID & "', '" & frmMain.dateNowMysql & "', '" & frmMain.getUserName & "')"

            com = New MySqlCommand(strSQL, conn)
            com.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public saveMode

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddReference.Click
        saveMode = "INSERT"
        doSetMode("ADD")
    End Sub


    Private Sub cmbFamily_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbFamily.KeyPress
        e.Handled = True
    End Sub


    Sub selectReference()
        txtArtNumber.Text = frmMain.readDB("art_number", "tblReference", "refe_name", txtReference.Text)
        txtLabelCode.Text = frmMain.readDB("label_code", "tblReference", "refe_name", txtReference.Text)
        cmbFamily.Text = frmMain.readDB("family", "tblReference", "refe_name", txtReference.Text)
        cmbACDC.Text = frmMain.readDB("acdc", "tblReference", "refe_name", txtReference.Text)
        nudJob.Value = frmMain.readDB("job_vision_id", "tblReference", "refe_name", txtReference.Text)
        If frmMain.readDB("job_vision_name", "tblMstrVision", "job_vision_id", nudJob.Value) <> "" Then
            txtJobNamec.Text = frmMain.readDB("job_vision_name", "tblMstrVision", "job_vision_id", nudJob.Value)
        Else
            txtJobNamec.Text = "NOT FOUND"
        End If
    End Sub


    Private Sub dgvReference_CellContentDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvReference.CellContentDoubleClick
        txtReference.Text = dgvReference.SelectedRows.Item(0).Cells(0).Value
        selectReference()

        doSetMode("READ")
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteReference.Click
        Dim resultDelete As DialogResult = MessageBox.Show("Anda yakin ingin menghapus data reference ini?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If resultDelete = DialogResult.Yes Then
            deleteReference()
            viewListReference("")
        End If
    End Sub

    Public Sub doSetModeGroup(ByVal vParamMode As String)
        btnEditGroup.Enabled = True
        btnDeleteGroup.Enabled = True
        Select Case vParamMode
            Case "READ"
                cmbSize.Enabled = False
                cmbVoltage.Enabled = False
                nudLevel.Enabled = False

                cmbSize.BackColor = SystemColors.Control
                cmbVoltage.BackColor = SystemColors.Control
                nudLevel.BackColor = SystemColors.Control

            Case "EDIT"
                cmbSize.Enabled = False
                cmbVoltage.Enabled = False
                nudLevel.Enabled = True

                cmbSize.BackColor = Color.White
                cmbVoltage.BackColor = Color.White
                nudLevel.BackColor = Color.White

            Case "ADD"
                cmbSize.Enabled = True
                cmbVoltage.Enabled = True
                nudLevel.Enabled = True

                cmbSize.BackColor = Color.White
                cmbVoltage.BackColor = Color.White
                nudLevel.BackColor = Color.White

                cmbSize.Text = ""
                cmbVoltage.Text = ""
                nudLevel.Value = 2

                btnEditGroup.Enabled = False
                btnDeleteGroup.Enabled = False
        End Select

    End Sub

    Public saveModeGroup

    Private Sub btnAddGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddGroup.Click
        saveModeGroup = "INSERT"
        doSetModeGroup("ADD")
    End Sub

    Private Sub btnEditGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditGroup.Click
        If cmbSize.Text <> "" Then
            saveModeGroup = "UPDATE"
            doSetModeGroup("EDIT")
        End If

    End Sub

    Private Sub btnDeleteGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteGroup.Click
        Dim resultDelete As DialogResult = MessageBox.Show("Anda yakin ingin menghapus data family ini?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If resultDelete = DialogResult.Yes Then
            deleteFamily()
            viewListFamily()
        End If
    End Sub

    Public Sub insertFamily()

        frmMain.queryDB("INSERT INTO tblFamily (family, ACDC, level, S1, S2, S3, S4, S5, S6, S7, created_date, created_by) VALUES ('" & cmbSize.Text & "', '" & cmbVoltage.Text & "', '" & nudLevel.Value & "', '" & sensor1 & "', '" & sensor2 & "', '" & sensor3 & "', '" & sensor4 & "', '" & sensor5 & "', '" & sensor6 & "',  '" & sensor7 & "', '" & frmMain.dateNowMysql & "', '" & frmMain.getUserName & "')")

    End Sub

    Public Sub viewListFamily()
        dgvRefeGroup.Rows.Clear()
        dgvRefeGroup.Columns.Clear()
        Dim strSQL As String
        Call connectServer()

        strSQL = "SELECT * FROM tblFamily"

        com = New MySqlCommand(strSQL, conn)
        Dim da As New MySqlDataAdapter(strSQL, conn)
        Dim ds As New DataSet
        'Dim reader As MySqlDataReader = com.ExecuteReader
        'Dim source As New BindingSource

        da.Fill(ds)
        da.Dispose()

        dgvRefeGroup.Columns.Add("Reference", "Reference")
        dgvRefeGroup.Columns.Add("Source", "Source")
        dgvRefeGroup.Columns.Add("Level", "Level")
        dgvRefeGroup.Columns.Add("Created Date", "Created Date")
        dgvRefeGroup.Columns.Add("Created By", "Created By")
        dgvRefeGroup.Columns.Add("Modified Date", "Modified Date")
        dgvRefeGroup.Columns.Add("Modified By", "Modified By")
        'dgvReference.Columns.Add("Up Full Travel", "Up Full Travel")
        'dgvReference.Columns.Add("Low Full Travel", "Low Full Travel")
        'dgvReference.Columns.Add("Up Release", "Up Release")
        'dgvReference.Columns.Add("Low Release", "Low Release")

        Dim baris = ds.Tables(0).Rows
        '  Dim statBox
        For part = 0 To baris.Count - 1

            dgvRefeGroup.Rows.Add(baris(part).Item("family").ToString, baris(part).Item("ACDC").ToString, baris(part).Item("level").ToString, baris(part).Item("created_date").ToString, baris(part).Item("created_by").ToString, baris(part).Item("modified_date").ToString, baris(part).Item("modified_by").ToString)

        Next                'If gdvPartData.Columns(0).HeaderText = "user_id" Then

    End Sub

    Public Sub viewListDataLabel(ByVal partNo As String)
        dgvDataLabel.ReadOnly = True
        dgvDataLabel.Rows.Clear()
        dgvDataLabel.Columns.Clear()
        Dim strSQL As String
        Call connectServer()

        If partNo = "" Then
            strSQL = "SELECT * FROM tblDataLabel"
        Else
            strSQL = "SELECT * FROM tblDataLabel WHERE label_code LIKE '" & partNo & "%'"
        End If

        com = New MySqlCommand(strSQL, conn)
        Dim da As New MySqlDataAdapter(strSQL, conn)
        Dim ds As New DataSet
        'Dim reader As MySqlDataReader = com.ExecuteReader
        'Dim source As New BindingSource

        da.Fill(ds)
        da.Dispose()

        dgvDataLabel.Columns.Add("Label Code", "Label Code")
        dgvDataLabel.Columns.Add("Label Part", "Label Part")
        dgvDataLabel.Columns.Add("File Template", "File Template")
        dgvDataLabel.Columns.Add("1ph_1", "1ph_1")
        dgvDataLabel.Columns.Add("1ph_2", "1ph_2")
        dgvDataLabel.Columns.Add("1ph_3", "1ph_3")
        dgvDataLabel.Columns.Add("1ph_4", "1ph_4")
        dgvDataLabel.Columns.Add("3ph_1", "3ph_1")
        dgvDataLabel.Columns.Add("3ph_2", "3ph_2")
        dgvDataLabel.Columns.Add("3ph_3", "3ph_3")
        dgvDataLabel.Columns.Add("3ph_4", "3ph_4")
        dgvDataLabel.Columns.Add("Max_Amp1", "Max_Amp1")
        dgvDataLabel.Columns.Add("Max_Amp2", "Max_Amp2")
        dgvDataLabel.Columns.Add("Max_Amp3", "Max_Amp3")
        dgvDataLabel.Columns.Add("Cont Current", "Cont Current")
        dgvDataLabel.Columns.Add("Line2", "Line2")
        dgvDataLabel.Columns.Add("Line3", "Line3")
        dgvDataLabel.Columns.Add("Line4", "Line4")
        dgvDataLabel.Columns.Add("Line5", "Line5")
        dgvDataLabel.Columns.Add("Line6", "Line6")
        dgvDataLabel.Columns.Add("KC_Logo", "KC_Logo")
        dgvDataLabel.Columns.Add("KC_No1", "KC_No1")
        dgvDataLabel.Columns.Add("KC_No2", "KC_No2")
        dgvDataLabel.Columns.Add("KC_No3", "KC_No3")
        dgvDataLabel.Columns.Add("Ith", "Ith")
        dgvDataLabel.Columns.Add("AC3_1", "AC3_1")
        dgvDataLabel.Columns.Add("AC3_2", "AC3_2")
        dgvDataLabel.Columns.Add("AC3_3", "AC3_3")
        dgvDataLabel.Columns.Add("JIS_1", "JIS_1")
        dgvDataLabel.Columns.Add("JIS_2", "JIS_2")
        dgvDataLabel.Columns.Add("Torque", "Torque")
        dgvDataLabel.Columns.Add("Strip", "Strip")
        dgvDataLabel.Columns.Add("Screw_Bit", "Screw_Bit")
        dgvDataLabel.Columns.Add("Max_Amp", "Max_Amp")
        dgvDataLabel.Columns.Add("Aux_Type", "Aux_Type")
        'dgvReference.Columns.Add("Up Full Travel", "Up Full Travel")
        'dgvReference.Columns.Add("Low Full Travel", "Low Full Travel")
        'dgvReference.Columns.Add("Up Release", "Up Release")
        'dgvReference.Columns.Add("Low Release", "Low Release")


        dgvDataLabel.Columns.Add("ul_logo", "ul_logo") ', "tblDataLabel", "label_code", codelabel)
        dgvDataLabel.Columns.Add("sa_logo", "sa_logo") ', "label_code", codelabel)
        dgvDataLabel.Columns.Add("eac_logo", "eac_logo") ', "label_code", codelabel)
        dgvDataLabel.Columns.Add("ctp_logo", "ctp_logo") ', "label_code", codelabel)
        dgvDataLabel.Columns.Add("ccc_logo", "ccc_logo") ', "label_code", codelabel)


        Dim baris = ds.Tables(0).Rows
        '  Dim statBox
        For part = 0 To baris.Count - 1

            dgvDataLabel.Rows.Add(baris(part).Item("label_code").ToString, baris(part).Item("label_part").ToString, baris(part).Item("file_template").ToString, baris(part).Item("1ph_1").ToString, baris(part).Item("1ph_2").ToString, baris(part).Item("1ph_3").ToString, baris(part).Item("1ph_4").ToString, baris(part).Item("3ph_1").ToString, baris(part).Item("3ph_2").ToString, baris(part).Item("3ph_3").ToString, baris(part).Item("3ph_4").ToString, baris(part).Item("max_amp1").ToString, baris(part).Item("max_amp2").ToString, baris(part).Item("max_amp3").ToString, baris(part).Item("cont_current").ToString, baris(part).Item("line_2").ToString, baris(part).Item("line_3").ToString, baris(part).Item("line_4").ToString, baris(part).Item("line_5").ToString, baris(part).Item("line_6").ToString, baris(part).Item("kc_logo").ToString, baris(part).Item("kc_no_1").ToString, baris(part).Item("kc_no_2").ToString, baris(part).Item("kc_no_3").ToString, baris(part).Item("ith").ToString, baris(part).Item("ac3_1").ToString, baris(part).Item("ac3_2").ToString, baris(part).Item("ac3_3").ToString, baris(part).Item("jis_1").ToString, baris(part).Item("jis_2").ToString, baris(part).Item("torque").ToString, baris(part).Item("strip").ToString, baris(part).Item("screw_bit").ToString, baris(part).Item("max_amp").ToString, baris(part).Item("aux_type").ToString, _
            baris(part).Item("ul_logo").ToString, _
            baris(part).Item("sa_logo").ToString, _
baris(part).Item("eac_logo").ToString, _
baris(part).Item("ctp_logo").ToString, _
baris(part).Item("ccc_logo").ToString)            ' baris(part).Item("created_date").ToString, baris(part).Item("created_by").ToString, baris(part).Item("modified_date").ToString, baris(part).Item("modified_by").ToString)

        Next                'If gdvPartData.Columns(0).HeaderText = "user_id" Then

    End Sub


    Public Sub viewListVision()
        dgvVision.Rows.Clear()
        dgvVision.Columns.Clear()
        Dim strSQL As String
        Call connectServer()

        strSQL = "SELECT * FROM tblMstrVision"

        com = New MySqlCommand(strSQL, conn)
        Dim da As New MySqlDataAdapter(strSQL, conn)
        Dim ds As New DataSet
        'Dim reader As MySqlDataReader = com.ExecuteReader
        'Dim source As New BindingSource

        da.Fill(ds)
        da.Dispose()

        dgvVision.Columns.Add("Job ID", "Job ID")
        dgvVision.Columns.Add("Job Name", "Job Name")
        dgvVision.Columns.Add("Description", "Description")
        dgvVision.Columns.Add("Created Date", "Created Date")
        dgvVision.Columns.Add("Created By", "Created By")
        dgvVision.Columns.Add("Modified Date", "Modified Date")
        dgvVision.Columns.Add("Modified By", "Modified By")
        dgvVision.Columns.Add("Flag", "Flag")
        'dgvReference.Columns.Add("Up Full Travel", "Up Full Travel")
        'dgvReference.Columns.Add("Low Full Travel", "Low Full Travel")
        'dgvReference.Columns.Add("Up Release", "Up Release")
        'dgvReference.Columns.Add("Low Release", "Low Release")

        Dim baris = ds.Tables(0).Rows
        '  Dim statBox
        For part = 0 To baris.Count - 1

            dgvVision.Rows.Add(baris(part).Item("job_vision_id").ToString, baris(part).Item("job_vision_name").ToString, baris(part).Item("description").ToString, baris(part).Item("created_date").ToString, baris(part).Item("created_by").ToString, baris(part).Item("modified_date").ToString, baris(part).Item("modified_by").ToString, baris(part).Item("modified_by").ToString)

        Next                'If gdvPartData.Columns(0).HeaderText = "user_id" Then

    End Sub

    Sub updateFamily()

        frmMain.queryDB("UPDATE tblFamily SET level = '" & nudLevel.Value & "', modified_date = '" & frmMain.dateNowMysql & "', modified_by = '" & frmMain.getUserName & "' WHERE family = '" & cmbSize.Text & "' AND ACDC = '" & cmbVoltage.Text & "'")

    End Sub

    Sub deleteFamily()

        frmMain.queryDB("DELETE FROM tblFamily WHERE family = '" & cmbSize.Text & "' ")

    End Sub


    Private Sub btnSaveFamily_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveFamily.Click


        If saveModeGroup = "INSERT" Then
            insertFamily()
            viewListFamily()
        ElseIf saveModeGroup = "UPDATE" Then
            updateFamily()
            viewListFamily()
        End If
        doSetModeGroup("READ")

    End Sub

    Private Sub cmbGroupReference_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.Handled = True
    End Sub



    Sub selectRefeGroup()


        nudLevel.Value = frmMain.readDB2("level", "tblFamily", "family", cmbSize.Text, "ACDC", cmbVoltage.Text)

    End Sub


    Private Sub dgvRefeGroup_CellContentDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvRefeGroup.CellContentDoubleClick
        cmbSize.Text = dgvRefeGroup.SelectedRows.Item(0).Cells(0).Value
        cmbVoltage.Text = dgvRefeGroup.SelectedRows.Item(0).Cells(1).Value

        selectRefeGroup()

        doSetModeGroup("READ")

    End Sub

    Sub viewData()
        Call connectServer()
        Dim strSQL = "SELECT * FROM tblReference WHERE refe_name LIKE '" & txtViewReference.Text & "%'"

        com = New MySqlCommand(strSQL, conn)

        Dim reader As MySqlDataReader = com.ExecuteReader

        If reader.HasRows = True Then

        Else

        End If

    End Sub

    Private Sub txtViewReference_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtViewReference.KeyPress
        If e.KeyChar = vbCr Then
            viewListReference(txtViewReference.Text)
        End If
    End Sub

    Private Sub btnViewReference_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewReference.Click
        viewListReference(txtViewReference.Text)
    End Sub

    Private Sub cmbSize_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSize.KeyPress
        e.Handled = True
    End Sub

    Private Sub cmbVoltage_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbVoltage.KeyPress
        e.Handled = True
    End Sub


    Private Sub txtNilaiFilter_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNilaiFilter.KeyPress
        If e.KeyChar = vbCr Then
            viewListDataLabel(txtNilaiFilter.Text)
        End If
    End Sub

    Private Sub btnFindDataLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindDataLabel.Click
        viewListDataLabel(txtNilaiFilter.Text)
    End Sub

    Private Sub btnAddDataLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddDataLabel.Click
        doSetModeDataLabel("ADD")
        saveModeLabel = "INSERT"
    End Sub

    Public Sub doSetModeDataLabel(ByVal vParamMode As String)
        btnEditDataLabel.Enabled = True
        btnDeleteDataLabel.Enabled = True
        Select Case vParamMode
            Case "READ"
                btnAddDataLabel.Enabled = True
                btnEditDataLabel.Enabled = True
                btnDeleteDataLabel.Enabled = True

                txtNilaiFilter.Enabled = True

            Case "EDIT"
                btnAddDataLabel.Enabled = False
                btnEditDataLabel.Enabled = False
                btnDeleteDataLabel.Enabled = False
                txtNilaiFilter.Enabled = False
            Case "ADD"
                For jmlkolom = 0 To dgvDataLabel.Columns.Count - 1
                    dgvDataLabel.Columns(jmlkolom).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                dgvDataLabel.Rows.Add(1)

                dgvDataLabel.ReadOnly = False
                For baris = 0 To dgvDataLabel.Rows.Count - 2
                    dgvDataLabel.Rows(baris).ReadOnly = True
                Next


                'ReadOnly = False

                txtNilaiFilter.Enabled = False

                btnAddDataLabel.Enabled = False
                btnEditDataLabel.Enabled = False
                btnDeleteDataLabel.Enabled = False
        End Select

    End Sub

    Private Sub dgvDataLabel_RowStateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowStateChangedEventArgs) Handles dgvDataLabel.RowStateChanged

    End Sub

    Public saveModeLabel

    Public Sub insertdatalabel()
        Try

            Dim barisTerakhir As Integer = dgvDataLabel.Rows.Count - 1


            Call connectServer()

            With dgvDataLabel.Rows(barisTerakhir)

                Dim strSQL = "INSERT INTO tblDataLabel VALUES ('" & .Cells(0).Value & "', '" & .Cells(1).Value & "', '" & .Cells(2).Value & "', '" & .Cells(3).Value & "', '" & .Cells(4).Value & "', '" & .Cells(5).Value & "', '" & .Cells(6).Value & "', '" & .Cells(7).Value & "', '" & .Cells(8).Value & "', '" & .Cells(9).Value & "', '" & .Cells(10).Value & "', '" & .Cells(11).Value & "', '" & .Cells(12).Value & "', '" & .Cells(13).Value & "', '" & .Cells(14).Value & "', '" & .Cells(15).Value & "', '" & .Cells(16).Value & "', '" & .Cells(17).Value & "', '" & .Cells(18).Value & "', '" & .Cells(19).Value & "', '" & .Cells(20).Value & "', '" & .Cells(21).Value & "', '" & .Cells(22).Value & "', '" & .Cells(23).Value & "', '" & .Cells(24).Value & "', '" & .Cells(25).Value & "', '" & .Cells(26).Value & "', '" & .Cells(27).Value & "', '" & .Cells(28).Value & "', '" & .Cells(29).Value & "', '" & .Cells(30).Value & "', '" & .Cells(31).Value & "', '" & .Cells(32).Value & "', '" & .Cells(33).Value & "', '" & .Cells(34).Value & "')"

                com = New MySqlCommand(strSQL, conn)
                com.ExecuteNonQuery()
            End With

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub updatedatalabel()
        Try

            Call connectServer()

            With dgvDataLabel.Rows(0)


                '                Dim strSQL = "INSERT INTO tblDataLabel VALUES ('" & .Cells(0).Value & "', '" & .Cells(1).Value & "', '" & .Cells(2).Value & "', '" & .Cells(3).Value & "', '" & .Cells(4).Value & "', '" & .Cells(5).Value & "', '" & .Cells(6).Value & "', '" & .Cells(7).Value & "', '" & .Cells(8).Value & "', '" & .Cells(9).Value & "', '" & .Cells(10).Value & "', '" & .Cells(11).Value & "', '" & .Cells(12).Value & "', '" & .Cells(13).Value & "', '" & .Cells(14).Value & "', '" & .Cells(15).Value & "', '" & .Cells(16).Value & "', '" & .Cells(17).Value & "', '" & .Cells(18).Value & "', '" & .Cells(19).Value & "', '" & .Cells(20).Value & "', '" & .Cells(21).Value & "', '" & .Cells(22).Value & "', '" & .Cells(23).Value & "', '" & .Cells(24).Value & "', '" & .Cells(25).Value & "', '" & .Cells(26).Value & "', '" & .Cells(27).Value & "', '" & .Cells(28).Value & "', '" & .Cells(29).Value & "', '" & .Cells(30).Value & "', '" & .Cells(31).Value & "', '" & .Cells(32).Value & "', '" & .Cells(33).Value & "', '" & .Cells(34).Value & "')"
                Dim strSQL = "UPDATE tblDataLabel SET label_code = '" & .Cells(0).Value & "', label_part = '" & .Cells(1).Value & "', file_template = '" & .Cells(2).Value & "', 1ph_1 = '" & .Cells(3).Value & "' , 1ph_2 = '" & .Cells(4).Value & "' , 1ph_3 = '" & .Cells(5).Value & "' , 1ph_4 = '" & .Cells(6).Value & "' , 3ph_1 = '" & .Cells(7).Value & "' , 3ph_2 = '" & .Cells(8).Value & "' , 3ph_3 = '" & .Cells(9).Value & "' , 3ph_4 = '" & .Cells(10).Value & "' , max_amp1 = '" & .Cells(11).Value & "' , max_amp2 = '" & .Cells(12).Value & "' , max_amp3 = '" & .Cells(13).Value & "' , cont_current = '" & .Cells(14).Value & "' , line_2 = '" & .Cells(15).Value & "' , line_3 = '" & .Cells(16).Value & "' , line_4 = '" & .Cells(17).Value & "' , line_5 = '" & .Cells(18).Value & "' , line_6 = '" & .Cells(19).Value & "' , kc_logo = '" & .Cells(20).Value & "' , kc_no_1 = '" & .Cells(21).Value & "' , kc_no_2 = '" & .Cells(22).Value & "' , kc_no_3 = '" & .Cells(23).Value & "' , ith = '" & .Cells(24).Value & "' , ac3_1 = '" & .Cells(25).Value & "' , ac3_2 = '" & .Cells(26).Value & "' , ac3_3 = '" & .Cells(27).Value & "' , jis_1 = '" & .Cells(28).Value & "' , jis_2 = '" & .Cells(29).Value & "' , torque = '" & .Cells(30).Value & "' , strip = '" & .Cells(31).Value & "' , screw_bit = '" & .Cells(32).Value & "' , max_amp = '" & .Cells(33).Value & "' , aux_type = '" & .Cells(34).Value & "' , eac_logo = '" & .Cells("eac_logo").Value & "' , ccc_logo = '" & .Cells("ccc_logo").Value & "' , ctp_logo = '" & .Cells("ctp_logo").Value & "' , ul_logo = '" & .Cells("ul_logo").Value & "' , sa_logo = '" & .Cells("sa_logo").Value & "' WHERE label_code = '" & dgvDataLabel.Rows(0).Cells(0).Value & "' "

                com = New MySqlCommand(strSQL, conn)
                com.ExecuteNonQuery()
            End With

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btnSaveDataLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveDataLabel.Click
        If saveModeLabel = "INSERT" Then
            insertdatalabel()
            viewListDataLabel("")
        ElseIf saveModeLabel = "UPDATE" Then
            updatedatalabel()
            viewListDataLabel("")
        End If
        doSetModeDataLabel("READ")

    End Sub

    Public Sub deleteDataLabel()
        Try

            Call connectServer()
            Dim strSQL = "DELETE FROM tblDataLabel WHERE label_code = '" & dgvDataLabel.SelectedRows(0).Cells(0).Value & "' "

            com = New MySqlCommand(strSQL, conn)
            com.ExecuteNonQuery()

            MsgBox("Delete Reference Done!", MsgBoxStyle.Information)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnDeleteDataLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteDataLabel.Click
        Dim resultDelete As DialogResult = MessageBox.Show("Anda yakin ingin menghapus data label ini?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If resultDelete = DialogResult.Yes Then
            deleteDataLabel()
            viewListDataLabel("")
        End If

    End Sub

    Sub exportReferenceToExcel(ByVal valPath As String) ' ByVal valExt As String)

        Dim StrExport As String = ""
        For Each C As DataGridViewColumn In dgvReference.Columns
            StrExport &= " " & C.HeaderText & ","
        Next
        StrExport = StrExport.Substring(0, StrExport.Length - 1)
        StrExport &= Environment.NewLine

        For Each R As DataGridViewRow In dgvReference.Rows
            For Each C As DataGridViewCell In R.Cells
                If Not C.Value Is Nothing Then
                    StrExport &= "" & C.Value.ToString & ","
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

        MsgBox("Data Reference has been download!", MsgBoxStyle.Information)

    End Sub

    Sub exportReferenceCSV()
        If dgvReference.RowCount = 0 Then
            MsgBox("No data exist!", MsgBoxStyle.Information)
        Else

            'ofdSaveLog.ShowDialog()
            sfdExcel.FileName = "Data Reference TeSys D"
            sfdExcel.AddExtension = True
            sfdExcel.DefaultExt = ".CSV"

            If sfdExcel.ShowDialog = DialogResult.OK Then
                exportReferenceToExcel(sfdExcel.FileName)
            End If
        End If
    End Sub











    Sub exportDataLabelToExcel(ByVal valPath As String) ' ByVal valExt As String)

        Dim StrExport As String = ""
        For Each C As DataGridViewColumn In dgvDataLabel.Columns
            StrExport &= " " & C.HeaderText & ","
        Next
        StrExport = StrExport.Substring(0, StrExport.Length - 1)
        StrExport &= Environment.NewLine

        For Each R As DataGridViewRow In dgvDataLabel.Rows
            For Each C As DataGridViewCell In R.Cells
                If Not C.Value Is Nothing Then
                    StrExport &= " " & Replace(C.Value.ToString, ",", ".") & ","
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

        MsgBox("Data Label has been download!", MsgBoxStyle.Information)

    End Sub

    Sub exportDataLabelCSV()
        If dgvDataLabel.RowCount = 0 Then
            MsgBox("No data exist!", MsgBoxStyle.Information)
        Else

            'ofdSaveLog.ShowDialog()
            sfdExcel.FileName = "Data Label TeSys D"
            sfdExcel.AddExtension = True
            sfdExcel.DefaultExt = ".CSV"

            If sfdExcel.ShowDialog = DialogResult.OK Then
                exportDataLabelToExcel(sfdExcel.FileName)
            End If
        End If
    End Sub
















    Private Sub btnExportReference_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportReference.Click
        exportReferenceCSV()
    End Sub

    Private Sub btnExportDataLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportDataLabel.Click
        exportDataLabelCSV()
    End Sub


    Private Sub btnEditDataLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditDataLabel.Click
        If dgvDataLabel.Rows.Count <> 1 Then
            MsgBox("FILTER DULU DATA YANG INGIN DIEDIT!", MsgBoxStyle.Information)
        Else
            doSetModeDataLabel("EDIT")
            saveModeLabel = "UPDATE"
            dgvDataLabel.ReadOnly = False

        End If
    End Sub

    Private Sub dgvDataLabel_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvDataLabel.KeyDown
        If e.KeyCode = Keys.Escape Then
            viewListDataLabel("")
            doSetModeDataLabel("READ")
        End If
    End Sub

    Private Sub frmReference_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Public Function ReadLineWithNumberFrom(ByVal filePath As String, ByVal lineNumber As Integer) As String
        Using file As New StreamReader(filePath)
            ' Skip all preceding lines: '
            For i As Integer = 1 To lineNumber - 1
                If file.ReadLine() Is Nothing Then
                    Throw New ArgumentOutOfRangeException("lineNumber")
                End If
            Next
            ' Attempt to read the line you're interested in: '
            Dim line As String = file.ReadLine()
            If line Is Nothing Then
                Throw New ArgumentOutOfRangeException("lineNumber")
            End If
            ' Succeded!
            Return line
        End Using
    End Function

    Dim pathReference As String
    Sub importReference()
        pathReference = "D:\Z\TeSys D Vision\Data Reference TeSys D.CSV"

        'get path file from openfiledialog
        'read every texcel record from path file
        'check row(0) excel in databaseu
        Dim data() As String = System.IO.File.ReadAllLines(pathReference)
        Dim refeExcel As String
        Dim artNumberExcel As String
        Dim labelCodeExcel As String
        Dim familyExcel As String
        Dim acdcExcel As String
        Dim jobIDExcel As String

        Dim qtyUpdateSuccess As Integer = 0
        Dim qtyInsertSuccess As Integer = 0
        Dim qtyUpdateFailed As Integer = 0
        Dim qtyInsertFailed As Integer = 0

        '        Exit Sub
        Dim line() As String = System.IO.File.ReadAllLines(pathReference)

        For record = 1 To line.Length - 1
            Dim atad() As String = Replace(line(record), " ", "").Split(",")

            refeExcel = atad(0)
            artNumberExcel = atad(1)
            labelCodeExcel = atad(2)
            familyExcel = atad(3)
            acdcExcel = atad(4)
            jobIDExcel = atad(5)

            prbReference.Value = (record / (line.Length - 1)) * 100

            Dim jobIDVision
            If frmMain.readDB("refe_name", "tblReference", "refe_name", refeExcel) <> "NULL" Then
                'if exist then update
                Try
                    jobIDVision = frmMain.readDB("job_vision_id", "tblMstrVision", "job_vision_id", jobIDExcel)
                    If jobIDVision = jobIDExcel Then
                        updateReference(refeExcel, artNumberExcel, labelCodeExcel, familyExcel, acdcExcel, jobIDExcel)
                        qtyUpdateSuccess = qtyUpdateSuccess + 1
                    Else
                        qtyUpdateFailed = qtyUpdateFailed + 1
                    End If

                Catch ex As Exception

                    qtyUpdateFailed = qtyUpdateFailed + 1

                End Try
                'if not then add
            Else
                Try
                    jobIDVision = frmMain.readDB("job_vision_id", "tblMstrVision", "job_vision_id", jobIDExcel)
                    If jobIDVision = jobIDExcel Then
                        insertReference(refeExcel, artNumberExcel, labelCodeExcel, familyExcel, acdcExcel, jobIDExcel)
                        qtyInsertSuccess = qtyInsertSuccess + 1
                    Else
                        qtyInsertFailed = qtyInsertFailed + 1
                    End If
                Catch ex As Exception
                    qtyInsertFailed = qtyInsertFailed + 1
                End Try
            End If

        Next

        MsgBox("- " & qtyInsertSuccess & " data berhasil ditambahkan" & vbCr & "- " & qtyUpdateSuccess & " data berhasil diupdate " & vbCr & "- " & qtyInsertFailed & " data gagal ditambahkan " & vbCr & "- " & qtyUpdateFailed & " data gagal diupdate", MsgBoxStyle.Information)
        prbReference.Value = 0
        viewListReference("")

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        importReference()
    End Sub

    Private Sub nudJob_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudJob.ValueChanged
        If frmMain.readDB("job_vision_name", "tblMstrVision", "job_vision_id", nudJob.Value) <> "" Then
            txtJobNamec.Text = frmMain.readDB("job_vision_name", "tblMstrVision", "job_vision_id", nudJob.Value)
        Else
            txtJobNamec.Text = "NOT FOUND"
        End If
    End Sub

    Public Sub doSetModeVision(ByVal vParamMode As String)
        btnEditVision.Enabled = True
        btnDeleteVision.Enabled = True
        Select Case vParamMode
            Case "READ"
                nudJobID.ReadOnly = True
                nudJobID.Enabled = False

                txtJobName.ReadOnly = True
                txtVisionDescription.ReadOnly = True

                nudJobID.BackColor = SystemColors.Control
                txtJobName.BackColor = SystemColors.Control
                txtVisionDescription.BackColor = SystemColors.Control

            Case "EDIT"
                nudJobID.ReadOnly = True
                nudJobID.Enabled = False

                txtJobName.ReadOnly = False
                txtVisionDescription.ReadOnly = False

                nudJobID.BackColor = Color.White
                txtJobName.BackColor = Color.White
                txtVisionDescription.BackColor = Color.White

            Case "ADD"
                nudJobID.ReadOnly = False
                nudJobID.Enabled = True

                txtJobName.ReadOnly = False
                txtVisionDescription.ReadOnly = False

                nudJobID.BackColor = Color.White
                txtJobName.BackColor = Color.White
                txtVisionDescription.BackColor = Color.White

                nudJobID.Value = 0
                txtJobName.Text = ""
                txtVisionDescription.Text = ""

        End Select

    End Sub

    Sub selectVision()
        txtJobName.Text = frmMain.readDB("job_vision_name", "tblMstrVision", "job_vision_id", nudJobID.Value)
        txtVisionDescription.Text = frmMain.readDB("description", "tblMstrVision", "job_vision_id", nudJobID.Value)
    End Sub

    Private Sub dgvVision_CellContentDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvVision.CellContentDoubleClick
        nudJobID.Value = dgvVision.SelectedRows.Item(0).Cells(0).Value
        selectVision()

        doSetModeVision("READ")

    End Sub
    Dim saveModeVision As String

    Private Sub btnEditVision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditVision.Click
        If nudJobID.Value <> 0 Then
            saveModeVision = "UPDATE"
            doSetModeVision("EDIT")
        End If
    End Sub

    Private Sub btnAddVision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddVision.Click
        If nudJobID.Value <> 0 Then
            saveModeVision = "INSERT"
            doSetModeVision("ADD")
        End If
    End Sub

    Public Sub insertVision(ByVal jobID As String, ByVal jobName As String, ByVal jobDesc As String)
        Try

            Call connectServer()
            Dim strSQL = "INSERT INTO tblMstrVision (job_vision_id, job_vision_name, description, created_date, created_by) VALUES ('" & jobID & "', '" & jobName & "', '" & jobDesc & "', '" & frmMain.dateNowMysql & "', '" & frmMain.getUserName & "')"

            com = New MySqlCommand(strSQL, conn)
            com.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub updateVision(ByVal jobID As String, ByVal jobName As String, ByVal jobDesc As String)
        Try
            Call connectServer()
            Dim strSQL = "UPDATE tblMstrVision SET job_vision_id = '" & jobID & "', job_vision_name = '" & jobName & "', description = '" & jobDesc & "', modified_date = '" & frmMain.dateNowMysql & "', modified_by = '" & frmMain.getUserName & "' WHERE job_vision_id = '" & jobID & "' "

            com = New MySqlCommand(strSQL, conn)
            com.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox("ERROR 2 : " & ex.Message)
        End Try

    End Sub

    Private Sub btnSaveVision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveVision.Click
        If saveModeVision = "INSERT" Then
            If nudJobID.Text <> 0 Then
                insertVision(nudJobID.Value, txtJobName.Text, txtVisionDescription.Text)
                viewListVision()
            Else
                MsgBox("ID tidak boleh 0!", MsgBoxStyle.Critical)
            End If
        ElseIf saveModeVision = "UPDATE" Then
            If nudJobID.Value <> 0 Then
                updateVision(nudJobID.Value, txtJobName.Text, txtVisionDescription.Text)
                viewListVision()
            Else
                MsgBox("ID tidak boleh 0!", MsgBoxStyle.Critical)
            End If
        End If
        doSetModeVision("READ")

    End Sub
End Class