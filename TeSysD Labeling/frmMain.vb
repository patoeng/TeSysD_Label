Imports System.IO
Imports System.Math
Imports MySql.Data.MySqlClient.MySqlConnection
Imports MySql.Data.MySqlClient
Imports System.Printing
Imports System.Net.Sockets

Public Class frmMain

    Dim SocketCam1 As New System.Net.Sockets.TcpClient()
    Dim SocketCam2 As New System.Net.Sockets.TcpClient()

#Region "PLC2"
    Public mw2_vision_job As Integer = 1
    Public mw2_job_trigger As Integer = 5
#End Region

    Public _PLC As New PLCModbusComms
    Public _PLC2 As New PLC2ModbusComms

    Public mw_vision_final_result As Integer = 42
    Public mw_vision_job As Integer = 43

    Public mw_frontcam_status As Integer = 47
    Public mw_backcam_status As Integer = 48

    Public mw_job_backcam_status As Integer = 51
    Public mw_job_frontcam_status As Integer = 52

    Public mw_vision_backcam_result As Integer = 53
    Public mw_vision_frontcam_result As Integer = 54

    Public mw_bypass_vision_check As Integer = 58
    Public mw_force_vision_result As Integer = 59

    Public mw_launching As Integer = 0
    Public mw_reset_Trigger As Integer = 1
    Public mw_trigger_print As Integer = 2
    Public mw_status_print As Integer = 3
    Public mw_st4_check As Integer = 4
    Public mw_st2_check As Integer = 6


    Public mw_st2_result As Integer = 18
    Public mw_st2_sensor As Integer = 21
    Public mw_error_code1 As Integer = 28
    Public mw_error_code2 As Integer = 29
    Public mw31 As Integer = 31
    Public mw_scanner_result As Integer = 32
    Public mw_scanner_trigger As Integer = 33
    Public mw_motor_level As Integer = 50
    Public mw_st4_result As Integer = 56
    Public mw_st4_result2 As Integer = 57
    Public mw_spec_size As Integer = 80
    Public mw_trigger_mw2 As Integer = 98

    Public getUserName As String
    Public getPassword As String
    Public sensor1ST2 As Integer
    Public sensor2ST2 As Integer
    Public sensor3ST2 As Integer
    Public sensor4ST2 As Integer
    Public sensor5ST2 As Integer
    Public sensor6ST2 As Integer
    Public sensor7ST2 As Integer
    Public printingLevel As String

    Public startTime As String = dateNowMysql()
    Public endTime As String = dateNowMysql()

    Sub checkLogin(ByVal user As String, ByVal pass As String)
        Select Case user
            Case "Operator"
                btnWorkOrder.Enabled = True
                btnReference.Enabled = False
                btnDataLog.Enabled = False
                btnMapping.Enabled = False
                btnSettings.Enabled = False
                btnUsers.Enabled = False
                chkScan.Checked = False
                chkScan.Enabled = False
                btnLogout.Enabled = True
                btnPrint.Visible = False

            Case "Engineer"
                btnWorkOrder.Enabled = True
                btnReference.Enabled = True
                btnDataLog.Enabled = True
                btnMapping.Enabled = True
                btnSettings.Enabled = True
                btnUsers.Enabled = False
                chkScan.Checked = False
                chkScan.Enabled = True
                btnLogout.Enabled = True
                btnPrint.Visible = True

            Case "Administrator"
                btnWorkOrder.Enabled = True
                btnReference.Enabled = True
                btnDataLog.Enabled = True
                btnMapping.Enabled = True
                btnSettings.Enabled = True
                btnUsers.Enabled = True
                chkScan.Checked = False
                chkScan.Enabled = True
                btnLogout.Enabled = True
                btnPrint.Visible = True

        End Select

        getUserName = user
        getPassword = pass

    End Sub

    Dim varInstructionSheet As String = "See delivered Instruction Sheet"

    Sub checkConnex(ByVal varRefe As String)
        If Mid(varRefe, 1, 3) = "CAD" Then
            If Mid(varRefe, 6, 1) <> "3" Then
                varInstructionSheet = "See delivered Instruction Sheet"
            Else
                varInstructionSheet = "See Instruction sheet W9137830401A55"
            End If

        ElseIf Mid(varRefe, 1, 4) = "LC1D" Then
            If Mid(varRefe, 5, 1) <> "T" Then
                If Mid(varRefe, 7, 1) <> "8" Then
                    If Mid(varRefe, 7, 1) <> "3" Then
                        varInstructionSheet = "See delivered Instruction Sheet"
                    Else
                        varInstructionSheet = "See Instruction sheet W9137830401A55"
                    End If
                Else
                    If Mid(varRefe, 8, 1) <> "3" Then
                        varInstructionSheet = "See delivered Instruction Sheet"
                    Else
                        varInstructionSheet = "See Instruction sheet W9137830401A55"
                    End If
                End If
            Else
                If Mid(varRefe, 8, 1) <> "8" Then
                    If Mid(varRefe, 8, 1) <> "3" Then
                        varInstructionSheet = "See delivered Instruction Sheet"
                    Else
                        varInstructionSheet = "See Instruction sheet W9137830401A55"
                    End If
                Else
                    If Mid(varRefe, 9, 1) <> "3" Then
                        varInstructionSheet = "See delivered Instruction Sheet"
                    Else
                        varInstructionSheet = "See Instruction sheet W9137830401A55"
                    End If
                End If


            End If

        End If


        If Mid(varRefe, 1, 7) = "LC1D098" Or Mid(varRefe, 1, 8) = "LC1DT203" Or Mid(varRefe, 1, 9) = "LC1DT1283" Or Mid(varRefe, 1, 8) = "LC1DT253" Or Mid(varRefe, 1, 8) = "LC1DT403" Or Mid(varRefe, 1, 8) = "LC1D2583" Then
            varInstructionSheet = "See delivered Instruction Sheet"
        End If

    End Sub

    Function checkSizeProduct(ByVal reference As String) As String

        Dim maxLength As Integer


        If reference.Contains("AB") Or reference.Contains("AA") Or reference.Contains("TQ") Then
            maxLength = reference.Length - 2
        Else
            maxLength = reference.Length
        End If

        For Case4Char = 100 To 199
            If reference.Contains("S" + Case4Char.ToString) Then
                maxLength = reference.Length - 4
            End If
        Next
        '4 POLE

        If Mid(reference, 1, 5) = "LC1DT" And Val(Mid(reference, 6, 2)) <= 25 And Mid(reference, 8, 1) <> "3" And Mid(reference, maxLength, 1) = "7" Then
            Return "S1-4P AC"

        ElseIf Mid(reference, 1, 5) = "LC1DT" And Val(Mid(reference, 6, 2)) > 25 And Mid(reference, 8, 1) <> "3" And Mid(reference, maxLength, 1) = "7" Then
            Return "S2-4P AC"

        ElseIf Mid(reference, 1, 5) = "LC1DT" And Val(Mid(reference, 6, 2)) <= 25 And Mid(reference, 8, 1) <> "3" And (Mid(reference, maxLength, 1) = "L" Or Mid(reference, maxLength, 1) = "D") Then
            Return "S1-4P DC"

        ElseIf Mid(reference, 1, 5) = "LC1DT" And Val(Mid(reference, 6, 2)) > 25 And Mid(reference, 8, 1) <> "3" And (Mid(reference, maxLength, 1) = "L" Or Mid(reference, maxLength, 1) = "D") Then
            Return "S2-4P DC"

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) < 18 And Mid(reference, 7, 1) = "8" And Mid(reference, maxLength, 1) = "7" Then
            Return "S1-4P AC"

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) >= 18 And Mid(reference, 7, 1) = "8" And Mid(reference, maxLength, 1) = "7" Then
            Return "S2-4P AC"

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) < 18 And Mid(reference, 7, 1) = "8" And (Mid(reference, maxLength, 1) = "L" Or Mid(reference, maxLength, 1) = "D") Then
            Return "S1-4P DC"

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) >= 18 And Mid(reference, 7, 1) = "8" And (Mid(reference, maxLength, 1) = "L" Or Mid(reference, maxLength, 1) = "D") Then
            Return "S2-4P DC"

            '3POLE

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) <= 18 And Mid(reference, 7, 1) <> "8" And Mid(reference, maxLength, 1) = "7" Then
            Return "S1-3P AC"

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) > 18 And Mid(reference, 7, 1) <> "8" And Mid(reference, maxLength, 1) = "7" Then
            Return "S2-3P AC"

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) <= 18 And Mid(reference, 7, 1) <> "8" And (Mid(reference, maxLength, 1) = "L" Or Mid(reference, maxLength, 1) = "D") Then
            Return "S1-3P DC"

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) > 18 And Mid(reference, 7, 1) <> "8" And (Mid(reference, maxLength, 1) = "L" Or Mid(reference, maxLength, 1) = "D") Then
            Return "S2-3P DC"

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) <= 18 And Mid(reference, 7, 1) <> "8" And (Mid(reference, maxLength, 1)) = "7" Then
            Return "S1-3P AC"

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) > 18 And Mid(reference, 7, 1) <> "8" And Mid(reference, maxLength, 1) = "7" Then
            Return "S2-3P AC"

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) <= 18 And Mid(reference, 7, 1) <> "8" And (Mid(reference, maxLength, 1) = "L" Or Mid(reference, maxLength, 1) = "D") Then
            Return "S1-3P DC"

        ElseIf Mid(reference, 1, 4) = "LC1D" And Mid(reference, 5, 1) <> "T" And Val(Mid(reference, 5, 2)) > 18 And Mid(reference, 7, 1) <> "8" And Mid(reference, maxLength, 1) = "7" Then
            Return "S2-3P DC"

            'CAD

        ElseIf Mid(reference, 1, 3) = "CAD" And Mid(reference, maxLength, 1) = "7" Or Mid(reference, 1, 3) = "DPE" Then
            Return "CAD32 AC"

        ElseIf Mid(reference, 1, 3) = "CAD" And (Mid(reference, maxLength, 1) = "L" Or Mid(reference, maxLength, 1) = "D") Then
            Return "CAD32 DC"

        End If

    End Function

    Public ipAddress As String

    Public Function getIPAddress() As String

    End Function

    Sub transmitDataCam1(ByVal comm As String)
        Dim serverStream As NetworkStream = SocketCam1.GetStream()

        Dim outStream As Byte() = System.Text.Encoding.ASCII.GetBytes(comm & vbCrLf)

        serverStream.Write(outStream, 0, outStream.Length)

        serverStream.Flush()

        ' Dim serverStream As NetworkStream = clientSocket.GetStream()

        Dim inStream(10024) As Byte

        serverStream.Read(inStream, 0, CInt(SocketCam1.ReceiveBufferSize))

        Dim returndata As String = System.Text.Encoding.ASCII.GetString(inStream)
        msg(returndata)

    End Sub

    Dim result As String

    Sub msg(ByVal mesg As String)
        result = ""
        result = mesg

        '        TextBox1.Text = TextBox1.Text + Environment.NewLine + " >> " + mesg

    End Sub

    Sub transmitDataCam2(ByVal comm As String)
        Dim serverStream As NetworkStream = SocketCam2.GetStream()

        Dim outStream As Byte() = System.Text.Encoding.ASCII.GetBytes(comm & vbCrLf)

        serverStream.Write(outStream, 0, outStream.Length)

        serverStream.Flush()

        ' Dim serverStream As NetworkStream = clientSocket.GetStream()

        Dim inStream(10024) As Byte

        serverStream.Read(inStream, 0, CInt(SocketCam2.ReceiveBufferSize))

        Dim returndata As String = System.Text.Encoding.ASCII.GetString(inStream)
        msg2(returndata)

    End Sub

    Dim result2 As String

    Sub msg2(ByVal mesg As String)
        result2 = ""
        result2 = mesg

        '        TextBox1.Text = TextBox1.Text + Environment.NewLine + " >> " + mesg

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


    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

        definePLC()

        writePLC(0, 0)

        responsiveForm()
        tmrReadPLC.Enabled = True
        If getUserName = "" Then
            doSetDisplay("Login")
        End If

        btnWorkOrder.Checked = True
        viewListWorkOrder("")

        Dim ipCam1 As String = ReadLineWithNumberFrom("config.ini", 1)
        Dim ipCam2 As String = ReadLineWithNumberFrom("config.ini", 2)

        SocketCam1.Connect(ipCam1, 23)
        If SocketCam1.Connected = False Then
            MsgBox("Kamera 1 tidak terkoneksi, periksa settingan jaringan " & ipCam1 & "!", MsgBoxStyle.Critical)
            End
        End If
        SocketCam2.Connect(ipCam2, 23)
        If SocketCam2.Connected = False Then
            MsgBox("Kamera 2 tidak terkoneksi, periksa settingan jaringan " & ipCam2 & "!", MsgBoxStyle.Critical)
            End
        End If



        transmitDataCam1("admin")
        transmitDataCam1("")

        If result = "Invalid Password" Then
            MsgBox("Username 'Admin' atau password '' untuk Kamera 1 salah!", MsgBoxStyle.Critical)
            End
        End If

        transmitDataCam2("admin")
        transmitDataCam2("")
        If result2 = "Invalid Password" Then
            MsgBox("Username 'Admin' atau password '' untuk Kamera 2 salah!", MsgBoxStyle.Critical)
            End
        End If

        gLabel = New LabelManager2.Application

    End Sub

    Private Sub tspFormControl_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tspFormControl.MouseLeave
        tspFormControl.Visible = False
    End Sub

    Sub exitApps()
        Dim resultDelete As DialogResult = MessageBox.Show("Are you sure want to exit this application?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If resultDelete = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

    Public monautomate As New classAutomateTCPIP.Automate
    Public monautomate2 As New classAutomateTCPIP.Automate



    Public Sub definePLC()
        monautomate.adresseIp = _PLC.ipPLC  '"192.168.178.10" ' IP ADDRESS PLC
        monautomate.nbMotsRTDB = 100 'JUMLAH MW
        monautomate.periodeScrutationMs = 150 'KECEPATAN PERPOLLING SEMUA MW 
        monautomate.adresseRTDBDansAutomate = 0
        monautomate.lanceScrutation()

        monautomate2.adresseIp = _PLC2.ipPLC ' "192.168.178.10" '"192.168.178.10" ' IP ADDRESS PLC
        monautomate2.nbMotsRTDB = 100 'JUMLAH MW
        monautomate2.periodeScrutationMs = 150 'KECEPATAN PERPOLLING SEMUA MW 
        monautomate2.adresseRTDBDansAutomate = 0
        monautomate2.lanceScrutation()

        _PLC.HostIP = _PLC.ipPLC 'ConfigurationManager.AppSettings("PLCAddress")
        _PLC.Port = 502
        _PLC.Timeout = 3000

        _PLC2.HostIP = _PLC2.ipPLC 'ConfigurationManager.AppSettings("PLCAddress")
        _PLC2.Port = 502
        _PLC2.Timeout = 3000

    End Sub

    Public Sub writePLC(ByVal xAddress As Integer, ByVal yValue As Integer)
        _PLC.WriteRegisterInt(xAddress, yValue)
    End Sub

    Public Sub writePLC2(ByVal xAddress As Integer, ByVal yValue As Integer)
        _PLC2.WriteRegisterInt(xAddress, yValue)
    End Sub

    Public Sub queryDB(ByVal query As String)
        Try

            Call connectServer()
            Dim strSQL = query

            com = New MySqlCommand(strSQL, conn)
            com.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox("queryDB  : " & ex.Message)
        End Try
    End Sub

    Function readDB(ByVal KOLOM As String, ByVal TABLE As String, ByVal KONDISI As String, ByVal NILAI_KONDISI As String) As String
        Try

            Call connectServer()
            Dim strSQL = "SELECT " & KOLOM & " FROM " & TABLE & " WHERE " & KONDISI & " = '" & NILAI_KONDISI & " ' "
            com = New MySqlCommand(strSQL, conn)
            Dim reader As MySqlDataReader = com.ExecuteReader

            reader.Read()

            If reader.HasRows Then
                Return reader(KOLOM).ToString
            End If
        Catch ex As Exception
            MsgBox("readDB : " & ex.Message)
        End Try

    End Function

    Function readDataBase(ByVal query As String, ByVal kolom As String) As String
        Try

            Call connectServer()
            Dim strSQL = query
            com = New MySqlCommand(strSQL, conn)
            Dim reader As MySqlDataReader = com.ExecuteReader

            reader.Read()

            If reader.HasRows Then
                Return reader(kolom).ToString
            Else
                Return "NULL"
            End If
        Catch ex As Exception
            MsgBox("readDatabase : " & ex.Message)
        End Try

    End Function

    Function readDB2(ByVal KOLOM As String, ByVal TABLE As String, ByVal KONDISI1 As String, ByVal NILAI_KONDISI1 As String, ByVal KONDISI2 As String, ByVal NILAI_KONDISI2 As String) As String
        Try
            Call connectServer()
            Dim strSQL = "SELECT " & KOLOM & " FROM " & TABLE & " WHERE " & KONDISI1 & " = '" & NILAI_KONDISI1 & " ' AND  " & KONDISI2 & " = '" & NILAI_KONDISI2 & " ' "
            com = New MySqlCommand(strSQL, conn)
            Dim reader As MySqlDataReader = com.ExecuteReader

            reader.Read()

            If reader.HasRows Then
                Return reader(KOLOM).ToString
            End If
        Catch ex As Exception
            MsgBox("readDB2 : " & ex.Message)
        End Try

    End Function

    Sub responsiveForm()
        Me.WindowState = FormWindowState.Maximized
        tspMenu.Left = (Me.Width - pnlBody.Width) / 2
        pnlBody.Left = tspMenu.Left
        pnlBody.Top = pnlHeader.Height
        'pnlFooter.Height = Me.Height - (pnlHeader.Height + pnlBody.Height)
        pnlBody.Height = Me.Height - (pnlHeader.Height + pnlFooter.Height)
        lblPowered.Left = (Me.Width / 2) - (lblPowered.Width / 2)

        dgvErrorCode.Columns.Add("Error Code", "Error Code")
        dgvErrorCode.Columns.Add("Error Date", "Error Date")

        frmMapping.txtMW28.Text = "65535"

        dgvTestStatus.Columns.Add("Test Result", "Test Result")
        dgvTestStatus.Columns.Add("Test Date", "Test Date")


        txtReadScan.Focus()
    End Sub

    Private Sub pnlHeader_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlHeader.MouseMove
        If MousePosition.Y < 20 Then
            tspFormControl.Visible = True
        Else
            tspFormControl.Visible = False
        End If

    End Sub

    Dim labelCode As String

    Sub viewReference(ByVal varRefeName As String)

        txtPrdReference.Text = varRefeName
        txtLaunchSize.Text = readDB("family", "tblReference", "refe_name", varRefeName)
        txtLaunchVoltage.Text = readDB("ACDC", "tblReference", "refe_name", varRefeName)

        labelCode = readDB("label_code", "tblReference", "refe_name", txtPrdReference.Text)

        loadLabel(readDB("label_code", "tblReference", "refe_name", txtPrdReference.Text))

        txtJobID.Text = readDB("job_vision_id", "tblReference", "refe_name", txtPrdReference.Text)
        txtJobName.Text = readDB("job_vision_name", "tblMstrVision", "job_vision_id", Val(txtJobID.Text))

        txtTemplate.Text = varTemplate 'readDB("file_template", "tblDataLabel", "label_code", codelabel) 'readDB("art_number", "tblReference", "refe_name", varRefeName)

        Try
            gDocGroup = gLabel.Documents.Open(Application.StartupPath & "\label\" & varTemplate & ".lab")
        Catch ex As Exception
            killLPPA()
            puyer(varTemplate)
            gLabel = New LabelManager2.Application
            gDocGroup = gLabel.Documents.Open(Application.StartupPath & "\label\" & varTemplate & ".lab")

        End Try

        gDocGroup.HorzPrintOffset = readDataBase("SELECT * FROM tblSetCodeSoft WHERE template = '" & Replace(varTemplate, " - new is", "") & "'", "horizontal") '0 'txtHorizontalOffset.Value
        gDocGroup.VertPrintOffset = readDataBase("SELECT * FROM tblSetCodeSoft WHERE template = '" & Replace(varTemplate, " - new is", "") & "'", "vertical") '0 'txtVerticalOffset.Value

        setValueCodesoft()

    End Sub

    Sub removeErrMesage()
        On Error Resume Next
        dgvErrorCode.Rows.RemoveAt(rowError("Template Mismatch! - Please check " & errTemplate & ".")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
    End Sub

    Function setJobVision() As Boolean
        If monautomate2.mwLectureAutomate(mw2_vision_job) <> Val(txtJobID.Text) Then
            writePLC2(mw2_vision_job, Val(txtJobID.Text))
        End If



    End Function

    Sub triggerJobVision()
        setJobVision()
        'writePLC(mw_vision_job, 0)
        'writePLC2(mw2_job_trigger, 0)

        'writePLC(mw_vision_job, 1)
        'writePLC2(mw2_job_trigger, 1)
    End Sub

    Sub viewWO(ByVal varPO As String)
        deletePrintJob()

        'txtPrdOrder.Text = readDB("prd_order", "tblWorkOrder", "prd_order", varPO)
        'txtPrdReference.Text = readDB("prd_reference", "tblWorkOrder", "prd_order", varPO)
        txtPrdQty.Text = Val(txtReadScan.Text) 'readDB("prd_qty", "tblWorkOrder", "prd_order", varPO)
        txtWOstatus.Text = "OPEN" 'readDB("status", "tblWorkOrder", "prd_order", varPO)
        txtPassQty.Text = "0" 'readDB("pass_qty", "tblWorkOrder", "prd_order", varPO)
        txtFailQty.Text = "0" 'readDB("fail_qty", "tblWorkOrder", "prd_order", varPO)

        '        viewReference(txtPrdReference.Text)

        '        triggerJobVision()

        frmLoading.Show()
        frmLoading.prgLoad.Value = 0


        transmitDataCam1("so0")
        frmLoading.prgLoad.Value = 10
        transmitDataCam1("sj" & txtJobID.Text)
        frmLoading.prgLoad.Value = 40
        transmitDataCam1("so1")
        frmLoading.prgLoad.Value = 50

        transmitDataCam2("so0")
        frmLoading.prgLoad.Value = 60
        transmitDataCam2("sj" & txtJobID.Text)
        frmLoading.prgLoad.Value = 90
        transmitDataCam2("so1")
        frmLoading.prgLoad.Value = 100
        frmLoading.Hide()

        setJobVision()



        If Val(txtJobID.Text) < 27 Then
            writePLC(mw_bypass_vision_check, 1)
        Else
            writePLC(mw_bypass_vision_check, 0)
        End If



        removeErrMesage()


        writePLC(50, getPrintingLevel(txtLaunchSize.Text, txtLaunchVoltage.Text))

        writePLC(0, 1)

        checkSizeLaunching()

    End Sub

    Sub getSizeProduct(ByVal varRefe As String)
        txtLaunchSize.Text = readDB("size", "tblReference", "refe_name", varRefe)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        exitApps()
    End Sub

    Public varTemplate As String
    Public varArtNumber As String
    Public varReference As String
    Public varNewLabelNo As String
    Public varNewPH11 As String
    Public varNewPH12 As String
    Public varNewPH13 As String
    Public varNewPH14 As String
    Public varNewPH31 As String
    Public varNewPH32 As String
    Public varNewPH33 As String
    Public varNewPH34 As String
    Public varNewMax1 As String
    Public varNewMax2 As String
    Public varNewMax3 As String
    Public varNewCont As String
    Public varNewAC31 As String
    Public varNewAC32 As String
    Public varNewAC33 As String
    Public varNewIth As String
    Public varNewLine2 As String '= readDB("line_2", "tblDataLabel", "label_code", codelabel)
    Public varNewTorque As String '= readDB("torque", "tblDataLabel", "label_code", codelabel)
    Public varNewStrip As String 'readDB("strip", "tblDataLabel", "label_code", codelabel)


    Public varNewLine3 As String '= readDB("line_3", "tblDataLabel", "label_code", codelabel)
    Public varNewLine4 As String '= readDB("line_4", "tblDataLabel", "label_code", codelabel)
    Public varNewLine5 As String '= readDB("line_5", "tblDataLabel", "label_code", codelabel)
    Public varNewLine6 As String '= readDB("line_6", "tblDataLabel", "label_code", codelabel)

    Public varKCLogo As String '= "icnew"

    Public varNewKC1 As String '= readDB("kc_no_1", "tblDataLabel", "label_code", codelabel)
    Public varNewKC2 As String '= readDB("kc_no_2", "tblDataLabel", "label_code", codelabel)

    Public varNewKC3 As String '= readDB("kc_no_3", "tblDataLabel", "label_code", codelabel)
    Public varNewJis1 As String '= readDB("jis_1", "tblDataLabel", "label_code", codelabel)
    Public varNewJis2 As String '= readDB("jis_2", "tblDataLabel", "label_code", codelabel)

    Public varNewScrewBit As String '= readDB("screw_bit", "tblDataLabel", "label_code", codelabel)
    Public varNewMaxAmp As String '= readDB("max_amp", "tblDataLabel", "label_code", codelabel)
    Public varNewAUX As String '= readDB("aux_type", "tblDataLabel", "label_code", codelabel)

    Public varNewKCNo1 As String '= readDB("kc_no_1", "tblDataLabel", "label_code", codelabel)
    Public varNewKCNo2 As String '= readDB("kc_no_2", "tblDataLabel", "label_code", codelabel)

    Public varULLogo As String
    Public varSALogo As String
    Public varEACLogo As String
    Public varCTPLogo As String
    Public varCCCLogo As String

    Sub loadLabel(ByVal codelabel)
        Try

            checkConnex(txtPrdReference.Text)

            varTemplate = readDB("file_template", "tblDataLabel", "label_code", codelabel) & " - new is"
            varArtNumber = readDB("art_number", "tblReference", "refe_name", txtPrdReference.Text)
            varReference = Replace(txtPrdReference.Text, "LV", "") 'readDB("filetemplate", "tblDataLabel", "label_code", codeLabel)
            varNewLabelNo = readDB("label_part", "tblDataLabel", "label_code", codelabel)

            varNewPH11 = readDB("1ph_1", "tblDataLabel", "label_code", codelabel)
            varNewPH12 = readDB("1ph_2", "tblDataLabel", "label_code", codelabel)
            varNewPH13 = readDB("1ph_3", "tblDataLabel", "label_code", codelabel)
            varNewPH14 = readDB("1ph_4", "tblDataLabel", "label_code", codelabel)

            varNewPH31 = readDB("3ph_1", "tblDataLabel", "label_code", codelabel)
            varNewPH32 = readDB("3ph_2", "tblDataLabel", "label_code", codelabel)
            varNewPH33 = readDB("3ph_3", "tblDataLabel", "label_code", codelabel)
            varNewPH34 = readDB("3ph_4", "tblDataLabel", "label_code", codelabel)

            varNewMax1 = readDB("max_amp1", "tblDataLabel", "label_code", codelabel)
            varNewMax2 = readDB("max_amp2", "tblDataLabel", "label_code", codelabel)
            varNewMax3 = readDB("max_amp3", "tblDataLabel", "label_code", codelabel)

            varNewCont = readDB("cont_current", "tblDataLabel", "label_code", codelabel)

            varNewAC31 = readDB("ac3_1", "tblDataLabel", "label_code", codelabel)
            varNewAC32 = readDB("ac3_2", "tblDataLabel", "label_code", codelabel)
            varNewAC33 = readDB("ac3_3", "tblDataLabel", "label_code", codelabel)
            varNewIth = readDB("ith", "tblDataLabel", "label_code", codelabel)

            varKCLogo = readDB("kc_logo", "tblDataLabel", "label_code", codelabel)

            varNewLine2 = readDB("line_2", "tblDataLabel", "label_code", codelabel)
            varNewTorque = readDB("torque", "tblDataLabel", "label_code", codelabel)
            varNewStrip = readDB("strip", "tblDataLabel", "label_code", codelabel)

            varNewLine3 = readDB("line_3", "tblDataLabel", "label_code", codelabel)
            varNewLine4 = readDB("line_4", "tblDataLabel", "label_code", codelabel)
            varNewLine5 = readDB("line_5", "tblDataLabel", "label_code", codelabel)
            varNewLine6 = readDB("line_6", "tblDataLabel", "label_code", codelabel)

            varNewKC1 = readDB("kc_no_1", "tblDataLabel", "label_code", codelabel)
            varNewKC2 = readDB("kc_no_2", "tblDataLabel", "label_code", codelabel)

            varNewKC3 = readDB("kc_no_3", "tblDataLabel", "label_code", codelabel)
            varNewJis1 = readDB("jis_1", "tblDataLabel", "label_code", codelabel)
            varNewJis2 = readDB("jis_2", "tblDataLabel", "label_code", codelabel)

            varNewScrewBit = readDB("screw_bit", "tblDataLabel", "label_code", codelabel)
            varNewMaxAmp = readDB("max_amp", "tblDataLabel", "label_code", codelabel)
            varNewAUX = readDB("aux_type", "tblDataLabel", "label_code", codelabel)

            varULLogo = readDB("ul_logo", "tblDataLabel", "label_code", codelabel)
            varSALogo = readDB("sa_logo", "tblDataLabel", "label_code", codelabel)
            varEACLogo = readDB("eac_logo", "tblDataLabel", "label_code", codelabel)
            varCTPLogo = readDB("ctp_logo", "tblDataLabel", "label_code", codelabel)
            varCCCLogo = readDB("ccc_logo", "tblDataLabel", "label_code", codelabel)

        Catch ex As Exception
            MsgBox("ERROR : Database hilang atau terhapus", MsgBoxStyle.Critical)
        End Try

    End Sub

    Dim errTemplate As String

    Public offsetCSHorizontal
    Public offsetCSVertical

    Sub getNewAUX(ByVal varRefe As String)
        If Mid(varRefe, 1, 5) = "CAD50" Then
            varNewAUX = "50"
        End If
    End Sub

    Sub setValueCodesoft()
        Try
            Dim strTemplate As String

            strTemplate = Replace(varTemplate, " - new is", "")

            errTemplate = strTemplate

            Dim var As LabelManager2.Variables  'LabelManager2.Variables

            var = gDocGroup.Variables

            With var

                .FormVariables.Item("InstructionSheet").Value = varInstructionSheet

                .FormVariables.Item("Art_number").Value = varArtNumber ' readDB("art_number", "tblReference", "refe_name", txtPrdReference.Text)
                .FormVariables.Item("Reference").Value = varReference '.Text 'readDB("filetemplate", "tblDataLabel", "label_code", codeLabel)
                .FormVariables.Item("NEW_LBLNO").Value = varNewLabelNo '("label_part", "tblDataLabel", "label_code", codeLabel)

                If ((strTemplate = "template1") Or (strTemplate = "template3")) Then

                    .FormVariables.Item("new_ph11").Value = varNewPH11 '("1ph_1", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("NEW_PH12").Value = varNewPH12 'readDB("1ph_2", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_ph13").Value = varNewPH13 '("1ph_3", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_ph14").Value = varNewPH14 'readDB("1ph_4", "tblDataLabel", "label_code", codeLabel)

                    .FormVariables.Item("new_ph31").Value = varNewPH31 'readDB("3ph_1", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_ph32").Value = varNewPH32 'readDB("3ph_2", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_ph33").Value = varNewPH33 '("3ph_3", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_ph34").Value = varNewPH34 'readDB("3ph_4", "tblDataLabel", "label_code", codeLabel)

                    .FormVariables.Item("NEW_MAX1").Value = varNewMax1 '("max_amp1", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_max2").Value = varNewMax2 'readDB("max_amp2", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_max3").Value = varNewMax3 'readDB("max_amp3", "tblDataLabel", "label_code", codeLabel)

                    .FormVariables.Item("new_cont").Value = varNewCont 'readDB("cont_current", "tblDataLabel", "label_code", codeLabel)

                    .FormVariables.Item("new_ac31").Value = varNewAC31 '("ac3_1", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_ac32").Value = varNewAC32 'readDB("ac3_2", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_ac33").Value = varNewAC33 'readDB("ac3_3", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_ith").Value = varNewIth '("ith", "tblDataLabel", "label_code", codeLabel)

                End If

                If strTemplate = "template3" Then
                    .FormVariables.Item("new_line2").Value = varNewLine2 'readDB("line_2", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_torque").Value = varNewTorque '("torque", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_strip").Value = varNewStrip 'readDB("strip", "tblDataLabel", "label_code", codeLabel)
                End If

                If ((strTemplate = "template1") Or (strTemplate = "template5")) Then

                    .FormVariables.Item("new_line3").Value = varNewLine3 'readDB("line_3", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_line4").Value = varNewLine4 'readDB("line_4", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_line5").Value = varNewLine5 'readDB("line_5", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_line6").Value = varNewLine6 'readDB("line_6", "tblDataLabel", "label_code", codeLabel)
                End If



                If strTemplate = "template1" Then
                    If varKCLogo = "visible" Then
                        .FormVariables.Item("KC_LOGO").Value = "icnew"
                    Else
                        .FormVariables.Item("KC_LOGO").Value = ""
                    End If
                    .FormVariables.Item("NEW_KC1").Value = varNewKC1 'readDB("kc_no_1", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("NEW_KC2").Value = varNewKC2 'readDB("kc_no_2", "tblDataLabel", "label_code", codeLabel)
                End If

                If ((strTemplate = "template1") Or (strTemplate = "template2")) Then

                    .FormVariables.Item("new_kc3").Value = varNewKC3 '("kc_no_3", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_jis1").Value = varNewJis1 'readDB("jis_1", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_jis2").Value = varNewJis2 'readDB("jis_2", "tblDataLabel", "label_code", codeLabel)

                End If

                If strTemplate = "template5" Then
                    If varNewScrewBit = "visible" Then
                        .FormVariables.Item("new_screwbit").Value = "screw_visible"
                    Else
                        .FormVariables.Item("new_screwbit").Value = ""
                        '("screw_bit", "tblDataLabel", "label_code", codeLabel)
                    End If
                    If varNewMaxAmp = "visible" Then
                        .FormVariables.Item("new_maxamp").Value = "max_visible" 'readDB("max_amp", "tblDataLabel", "label_code", codeLabel)
                    Else
                        .FormVariables.Item("new_maxamp").Value = "" 'readDB("max_amp", "tblDataLabel", "label_code", codeLabel)

                    End If

                    getNewAUX(txtPrdReference.Text)

                    .FormVariables.Item("new_aux").Value = varNewAUX 'readDB("aux_type", "tblDataLabel", "label_code", codeLabel)
                End If

                If strTemplate = "template6" Then
                    .FormVariables.Item("NEW_KC1").Value = varNewKC1 'readDB("kc_no_1", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("NEW_KC2").Value = varNewKC2 'readDB("kc_no_2", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_ac31").Value = varNewAC31 'readDB("ac3_1", "tblDataLabel", "label_code", codeLabel)
                    .FormVariables.Item("new_ac32").Value = varNewAC32 'readDB("ac3_2", "tblDataLabel", "label_code", codeLabel)
                End If

                If varULLogo = "visible" Then
                    .FormVariables.Item("ul_logo").Value = "ul_logo"
                Else
                    .FormVariables.Item("ul_logo").Value = ""
                End If

                If varSALogo = "visible" Then
                    .FormVariables.Item("sa_logo").Value = "sa_logo"
                Else
                    .FormVariables.Item("sa_logo").Value = ""
                End If

                If varEACLogo = "visible" Then
                    .FormVariables.Item("eac_logo").Value = "eac_logo"
                Else
                    .FormVariables.Item("eac_logo").Value = ""
                End If

                If varCTPLogo = "visible" Then
                    .FormVariables.Item("ctp_logo").Value = "ctp_logo"
                Else
                    .FormVariables.Item("ctp_logo").Value = ""
                End If

                If varCCCLogo = "visible" Then
                    .FormVariables.Item("ccc_logo").Value = "ccc_logo"
                Else
                    .FormVariables.Item("ccc_logo").Value = ""
                End If

            End With
        Catch ex As Exception
            killLPPA()
            puyer(varTemplate)
            gLabel = New LabelManager2.Application
            '    gDocGroup = gLabel.Documents.Open(Application.StartupPath & "\original label\" & varTemplate & ".lab")
            gDocGroup = gLabel.Documents.Open(Application.StartupPath & "\label\" & varTemplate & ".lab")
            setValueCodesoft()
        End Try
    End Sub

    Public Sub printLabel(ByVal codeLabel As String)

        Dim strTemplate As String

        strTemplate = varTemplate

        errTemplate = strTemplate

        Try

            gDocGroup.PrintDocument(1)

            '            gDocGroup.Close()

        Catch ex As Exception

            counterPuyer = counterPuyer + 1

            If counterPuyer < 3 Then
                puyer(errTemplate)
                printLabel(codeLabel)
            Else
                MsgBox("Template Missmatch! - Please check " & strTemplate & ".", MsgBoxStyle.Critical)

                dgvErrorCode.Rows.Insert(0, "Template Mismatch! - Please check " & strTemplate & ".", dateNowMysql)
                txtPrdOrder.Text = ""
                counterPuyer = 0
            End If
            '            MsgBox("ERROR PRINT : " & ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Dim counterPuyer As Integer = 0

    Sub killLPPA()
        For Each p As Process In System.Diagnostics.Process.GetProcessesByName("Lppa")
            p.Kill()
        Next

    End Sub


    Sub puyer(ByVal template)
        On Error Resume Next

        FileCopy(Application.StartupPath & "\original label\" & template & ".lab", Application.StartupPath & "\label\" & template & ".lab")

    End Sub

    Sub resetTriggerMW1()

        writePLC(1, 0)

        frmMapping.txtMW1.Text = "0"

    End Sub

    Private Sub btnRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRead.Click

        readscan()

    End Sub

    Private Sub txtPrdOrder_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPrdOrder.TextChanged
        If txtPrdOrder.Text = "" Then
            btnCodesoft.Enabled = False
            lblInstruction.Text = "SILAHKAN SCAN BARCODE PRODUCT ORDER!"
            resetScan()
        Else
            btnCodesoft.Enabled = True
        End If
    End Sub

    Private Sub ctrMinimize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctrMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub ctrClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctrClose.Click
        exitApps()
    End Sub

    Private Sub ctrMinimize_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctrMinimize.MouseLeave
        tspFormControl.Visible = False
    End Sub

    Function checkWO(ByVal prdNumber As String) As Boolean
        If readDB("prd_order", "tblWorkOrder", "prd_order", prdNumber) = prdNumber Then
            Return True
        Else
            Return False
        End If

    End Function

    Function checkReference(ByVal refeName As String) As Boolean
        If readDB("refe_name", "tblReference", "refe_name", refeName) = refeName Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function dateNowMysql() As String
        Return Now.ToString("yyyy-MM-dd HH-mm-ss")
    End Function

    Sub insertWO()
        '        queryDB("INSERT INTO tblWorkOrder (prd_order, prd_reference, prd_qty, launch_date) VALUES ('" & txtPrdOrder.Text & "', '" & txtPrdReference.Text & "', '" & txtPrdQty.Text & "', '" & dateNowMysql() & "')")
        viewWO(txtPrdOrder.Text)
    End Sub

    Sub scanBarcodePrdOrder(ByVal scanData As String)

        If Mid(scanData, 1, 3) = "220" And scanData.Length = 10 Then
            txtPrdOrder.Text = scanData
            lblInstruction.Text = "SILAHKAN SCAN BARCODE REFERENCE!"
            '                lblInstruction.Text = "PRODUCT ORDER YANG DIMASUKKAN SALAH"
            '            End If
        Else
            lblInstruction.Text = "FORMAT BARCODE PRODUCT ORDER SALAH!"
        End If

    End Sub

    Sub scanBarcodeReference(ByVal scanData As String)
        If checkReference(scanData) = True Then
            txtPrdReference.Text = scanData
            viewReference(txtPrdReference.Text) '
            lblInstruction.Text = "SILAHKAN SCAN BARCODE QUANTITY!"
        Else
            lblInstruction.Text = "FORMAT BARCODE REFERENCE SALAH!"
        End If

    End Sub

    Function checkKarakterNomor(ByVal karakter As Integer) As Boolean
        Try
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Sub scanBarcodeQuantity(ByVal scanData As String)
        'If btnSetVision.Visible = True Then
        '    Exit Sub
        'End If

        Try
            If scanData.Length = 6 And checkKarakterNomor(scanData) = True Then
                If checkWO(scanData) = True Then
                    lblInstruction.Text = "MASUKKAN PRODUCT KE CONVEYOR!"
                    viewWO(scanData)
                Else
                    txtPrdQty.Text = scanData
                    lblInstruction.Text = "MASUKKAN PRODUCT KE CONVEYOR!"
                    insertWO()
                End If
            Else
                lblInstruction.Text = "FORMAT BARCODE QUANTITY SALAH!"
            End If
        Catch ex As Exception
            lblInstruction.Text = "FORMAT BARCODE QUANTITY SALAH!"
        End Try
    End Sub



    Sub readscan()
        If monautomate.mwLectureAutomate(mw_frontcam_status) = 0 Or monautomate.mwLectureAutomate(mw_backcam_status) = 0 Then
            If lblInstruction.Text.Contains("BARCODE PRODUCT ORDER") Then
                lblInstruction.Text = "VISION NOT READY, TIDAK BISA SCAN BARCODE PRODUCT ORDER!"
            ElseIf lblInstruction.Text.Contains("BARCODE REFERENCE") Then
                lblInstruction.Text = "VISION NOT READY, TIDAK BISA SCAN BARCODE REFERENCE!"
            ElseIf lblInstruction.Text.Contains("BARCODE QUANTITY") Then
                lblInstruction.Text = "VISION NOT READY, TIDAK BISA SCAN BARCODE QUANTITY!"
            End If
            Exit Sub
        End If


        If txtReadScan.Text <> "" Then
            If lblInstruction.Text.Contains("BARCODE PRODUCT ORDER") Then
                scanBarcodePrdOrder(txtReadScan.Text)
            ElseIf lblInstruction.Text.Contains("BARCODE REFERENCE") Then
                scanBarcodeReference(txtReadScan.Text)
            ElseIf lblInstruction.Text.Contains("BARCODE QUANTITY") Then
                scanBarcodeQuantity(txtReadScan.Text)

            End If
            txtReadScan.Text = ""
        End If

    End Sub

    Private Sub txtReadScan_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReadScan.KeyPress
        If e.KeyChar = vbCr Then
            readscan()
        End If
    End Sub

    Private Sub chkScan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkScan.CheckedChanged
        If chkScan.Checked = True Then
            tmrResetScan.Enabled = False
            btnViewWO.Enabled = True
            'txtReadScan.ReadOnly = False
        Else
            tmrResetScan.Enabled = True
            btnViewWO.Enabled = False
            'txtReadScan.ReadOnly = False
        End If
    End Sub

    Private Sub txtReadScan_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReadScan.Leave
        txtReadScan.Focus()
    End Sub

    Private Sub tmrResetScan_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrResetScan.Tick
        If txtReadScan.Text <> "" Then
            If chkScan.Checked = False Then
                'txtReadScan.Text = ""
            End If
        End If
    End Sub

    Private Sub dgvWorkOrder_CellContentDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWorkOrder.CellContentDoubleClick
        txtReadScan.Text = dgvWorkOrder.SelectedRows.Item(0).Cells(0).Value()
        readscan()
        frmWorkOrder.Close()
    End Sub

    Sub checkSizeLaunching()

        If Mid(txtLaunchSize.Text, 1, 2) = "S2" Then ' & txtLaunchVoltage.Text Then
            If txtLaunchSize.Text = "S2-3P" And txtLaunchVoltage.Text = "AC" Then
                writePLC(80, 1)
            Else
                writePLC(80, 2)
            End If
        Else
            If txtLaunchSize.Text = "S1-4P" And txtLaunchVoltage.Text = "DC" Then
                writePLC(80, 2)
            Else
                writePLC(80, 1)
            End If
        End If

    End Sub

    Function getPrintingLevel(ByVal family As String, ByVal ACDC As String) As String
        Return readDataBase("SELECT * FROM tblFamily WHERE family = '" & family & "' AND ACDC = '" & ACDC & "'", "level")
        'txtDetectSize.Text = readDataBase("SELECT * FROM tblFamily WHERE s1 =  ' " & s1 & " ' AND s2 =  ' " & s2 & " ' AND s3 =  ' " & s3 & " ' AND s4 =  ' " & s4 & " ' AND s5 =  ' " & s5 & " ' AND s6 =  ' " & s6 & " ' AND s7 =  ' " & s7 & " '", "family")
    End Function

    Public Sub doSetDisplay(ByVal vstrParam As String)
        pnlBody.Controls.Clear()
        Select Case vstrParam
            Case "Login"

                frmLogin.responsiveForm()
                btnWorkOrder.Enabled = False
                btnReference.Enabled = False
                btnDataLog.Enabled = False
                btnMapping.Enabled = False
                btnSettings.Enabled = False
                btnUsers.Enabled = False

                btnLogout.Enabled = False

                pnlBody.Controls.Add(frmLogin.pnlMain)
                btnWorkOrder.Checked = False
                btnReference.Checked = False
                btnDataLog.Checked = False
                btnUsers.Checked = False

                txtReadScan.Text = ""
                chkScan.Checked = False

                txtPrdOrder.Text = ""
                txtPassQty.Text = ""
                txtFailQty.Text = ""

            Case "Work Order"

                If txtPrdOrder.Text = "" Then
                    lblInstruction.Text = "SILAHKAN SCAN BARCODE PRODUCT ORDER!"
                End If

                pnlBody.Controls.Add(pnlWorkOrder)
                btnWorkOrder.Checked = True
                btnReference.Checked = False
                btnDataLog.Checked = False
                btnMapping.Checked = False
                btnSettings.Checked = False
                btnUsers.Checked = False

                chkScan.Checked = False

            Case "Reference"
                pnlBody.Controls.Add(frmReference.pnlMain)
                btnWorkOrder.Checked = False
                btnReference.Checked = True
                btnDataLog.Checked = False
                btnMapping.Checked = False
                btnSettings.Checked = False
                btnUsers.Checked = False

            Case "Data Log"
                pnlBody.Controls.Add(frmDataLog.pnlMain)
                btnWorkOrder.Checked = False
                btnReference.Checked = False
                btnDataLog.Checked = True
                btnMapping.Checked = False
                btnSettings.Checked = False
                btnUsers.Checked = False

            Case "Debug"
                pnlBody.Controls.Add(frmMapping.pnlMain)
                btnWorkOrder.Checked = False
                btnReference.Checked = False
                btnDataLog.Checked = False
                btnMapping.Checked = True
                btnSettings.Checked = False
                btnUsers.Checked = False

            Case "Settings"
                'pnlBody.Controls.Add(frmSettings.pnlMain)
                btnWorkOrder.Checked = False
                btnReference.Checked = False
                btnDataLog.Checked = False
                btnMapping.Checked = False
                btnSettings.Checked = True
                btnUsers.Checked = False

            Case "User Management"
                frmPassword.responsiveForm()
                pnlBody.Controls.Add(frmPassword.pnlMain)
                btnWorkOrder.Checked = False
                btnReference.Checked = False
                btnDataLog.Checked = False
                btnMapping.Checked = False
                btnSettings.Checked = False
                btnUsers.Checked = True

        End Select

    End Sub

    Sub resetScan()
        writePLC(0, 0)
        txtPrdOrder.Text = ""
        txtPrdReference.Text = ""
        txtPrdQty.Text = ""
        txtWOstatus.Text = ""
        txtPassQty.Text = ""
        txtFailQty.Text = ""
        txtLaunchSize.Text = ""
        txtLaunchVoltage.Text = ""
        txtDetectSize.Text = ""

        txtJobID.Text = ""
        txtJobName.Text = ""

        txtTemplate.Text = ""
        txtDetectVision.Text = ""

        txtDetectSize.BackColor = Color.White

        txtDetectVision.BackColor = Color.White

    End Sub

    Sub deletePrintJob()
        'Dim psi As New ProcessStartInfo(Application.StartupPath & "/DeletePrintJobs.bat")
        'psi.RedirectStandardError = True
        'psi.RedirectStandardOutput = True
        'psi.CreateNoWindow = False
        'psi.WindowStyle = ProcessWindowStyle.Hidden
        'psi.UseShellExecute = False

        'Dim process As Process = process.Start(psi)
    End Sub

    Private Sub btnWorkOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWorkOrder.Click
        If txtPrdOrder.Text <> "" Then
            If btnWorkOrder.Checked = True Then
                Dim resultDelete As DialogResult = MessageBox.Show("Yakin ingin change series?", "Change Series", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If resultDelete = DialogResult.Yes Then
                    resetScan()
                End If
            End If
        End If
        doSetDisplay("Work Order")
    End Sub

    Private Sub lblInstruction_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblInstruction.TextChanged
        If lblInstruction.Text.Contains("BARCODE PRODUCT ORDER") Then
            pctTicket.ImageLocation = Application.StartupPath & "/images/BWI/PO.JPG"
        ElseIf lblInstruction.Text.Contains("BARCODE REFERENCE") Then
            pctTicket.ImageLocation = Application.StartupPath & "/images/BWI/MATERIAL.JPG"
        ElseIf lblInstruction.Text.Contains("BARCODE QUANTITY") Then
            pctTicket.ImageLocation = Application.StartupPath & "/images/BWI/QTY.JPG"
        ElseIf lblInstruction.Text.Contains("CONVEYOR") Then
            If txtLaunchVoltage.Text = "AC" Then
                pctTicket.ImageLocation = Application.StartupPath & "/images/BWI/PRODUCT AC.JPG"
            Else
                pctTicket.ImageLocation = Application.StartupPath & "/images/BWI/PRODUCT DC.JPG"
            End If

        End If
    End Sub

    Public wip As Integer
    Public st2Check As String
    Public st4Check As String

    Sub recordST2Pass()
        If Val(frmMapping.txtMW0.Text) <> 0 Then
            Try
                wip = selectWIPTester(txtPrdOrder.Text) + 1

                queryDB("INSERT INTO tblLogTester (prd_order, prd_reference, wip, st2_check, st4_check, final_result, start_time) VALUES ('" & txtPrdOrder.Text & "', '" & txtPrdReference.Text & "', '" & wip & "', 'PASS', 'NOT FINISH', 'NOT FINISH', '" & startTime & "')")

                dgvTestStatus.Sort(dgvTestStatus.Columns(1), System.ComponentModel.ListSortDirection.Descending)

                dgvTestStatus.Rows.Add("ST2 PASS", dateNowMysql)

            Catch ex As Exception
                MsgBox("recordST2Pass : " & ex.Message)
            End Try
        End If

    End Sub

    Sub recordST2Fail()
        If Val(frmMapping.txtMW0.Text) <> 0 Then
            Try
                wip = selectWIPTester(txtPrdOrder.Text) + 1

                queryDB("INSERT INTO tblLogTester (prd_order, prd_reference, wip, st2_check, st4_check, final_result) VALUES ('" & txtPrdOrder.Text & "', '" & txtPrdReference.Text & "', " & wip & ", 'FAIL', 'FAIL', 'FAIL')")

                dgvTestStatus.Sort(dgvTestStatus.Columns(1), System.ComponentModel.ListSortDirection.Descending)

                dgvTestStatus.Rows.Add("ST2 FAIL", dateNowMysql)

            Catch ex As Exception
                MsgBox("recordST2Fail : " & ex.Message)
            End Try

        End If
    End Sub

    Function minWIPUpdate(ByVal prdOrder As String) As String
        Dim updateLog As String = readDataBase("SELECT MAX(wip) as wipx FROM tblLogTester WHERE prd_order = '" & prdOrder & "' AND st4_check = 'NOT FINISH'", "wipx")
        If updateLog = "NULL" Then
            Return "0"
        Else
            Return updateLog
        End If

    End Function

    Function minWIPUpdateTIME() As String
        Dim updateLog As String = readDataBase("SELECT MAX(wip) as wipx FROM tblLogTester WHERE prd_order = '2202122222' AND st4_check = 'PASS'", "wipx")
        If updateLog = "NULL" Then
            Return "0"
        Else
            Return updateLog
        End If

    End Function

    Sub updateEndTime()
        If Val(frmMapping.txtMW0.Text) <> 0 Then
            Try
                Dim WIPX
                WIPX = minWIPUpdateTIME()

                queryDB("UPDATE tblLogTester SET end_time = '" & endTime & "' WHERE prd_order = '" & txtPrdOrder.Text & "' AND wip = '" & WIPX & "' ")

            Catch ex As Exception
                MsgBox("recordST4Pass : " & ex.Message)
            End Try
        End If

    End Sub

    Sub recordST4Pass()
        If Val(frmMapping.txtMW0.Text) <> 0 Then
            Try
                Dim WIPX
                WIPX = minWIPUpdate(txtPrdOrder.Text)

                queryDB("UPDATE tblLogTester SET st4_check = 'PASS', final_result = 'PASS' WHERE prd_order = '" & txtPrdOrder.Text & "' AND wip = '" & WIPX & "' ")

                dgvTestStatus.Sort(dgvTestStatus.Columns(1), System.ComponentModel.ListSortDirection.Descending)

                dgvTestStatus.Rows.Add("ST4 PASS", dateNowMysql)

            Catch ex As Exception
                MsgBox("recordST4Pass : " & ex.Message)
            End Try
        End If
    End Sub

    Sub recordST4Fail()
        If Val(frmMapping.txtMW0.Text) <> 0 Then
            Try
                Dim WIPX
                WIPX = minWIPUpdate(txtPrdOrder.Text)

                queryDB("UPDATE tblLogTester SET st4_check = 'FAIL', final_result = 'FAIL' WHERE prd_order = '" & txtPrdOrder.Text & "' AND wip = '" & WIPX & "' ")

                dgvTestStatus.Sort(dgvTestStatus.Columns(1), System.ComponentModel.ListSortDirection.Descending)

                dgvTestStatus.Rows.Add("ST4 FAIL", dateNowMysql)

            Catch ex As Exception
                MsgBox("recordST4Fail : " & ex.Message)
            End Try
        End If
    End Sub

    Dim valST2
    Dim valST4

    Dim valTestresults

    Public varpath As String

    Function selectDataLog(ByVal SONum As String, ByVal wip As Integer, ByVal field As String) As String
        Call connectServer()
        Dim strSQL = "SELECT * FROM tblLogTester WHERE prd_order = '" & SONum & "' AND wip = '" & wip & "'"
        com = New MySqlCommand(strSQL, conn)
        Dim reader As MySqlDataReader = com.ExecuteReader

        reader.Read()

        If reader.HasRows Then
            Return reader(field).ToString
        End If

    End Function

    Sub createLogFolder()
        ' BERDASARKAN MINGGU        varpath = Application.StartupPath & "\Data Log\" & Now.ToString("yy") & Round(Val((Now.DayOfYear + 4) / 7))

        varpath = Application.StartupPath & "\Data Log\" & Now.ToString("MMMM") & " " & Now.Year ' berdasarkan tanggal

        If (Not System.IO.Directory.Exists(varpath)) Then
            Directory.CreateDirectory(varpath)
        End If

    End Sub

    Function maxIncrementID() As String
        Call connectServer()
        Dim strSQL = "SELECT MAX(wip) as wipx FROM tblLogTester WHERE prd_order = '" & txtPrdOrder.Text & "'"

        com = New MySqlCommand(strSQL, conn)
        Dim reader As MySqlDataReader = com.ExecuteReader

        reader.Read()

        If reader.HasRows Then
            Return reader("wipx").ToString
        Else
            Return "0"
        End If
    End Function

    Sub createFileLog()
        createLogFolder()

        ' BERDASARKAN MINGGU        Dim varFile As String = (Application.StartupPath & "\Data Log\" & Now.ToString("yy") & Round(Val((Now.DayOfYear + 4) / 7)) & "\" & Now.ToString("yy") & Round(Val((Now.DayOfYear + 4) / 7)) & Now.ToString("dd") & ".log")

        Dim varFile As String = (Application.StartupPath & "\Data Log\" & Now.ToString("MMMM") & " " & Now.Year & "\" & Now.ToString("MM") & "-" & Now.ToString("dd") & "-" & Now.Year & ".log")

        ' berdasarkan tanggal

        valST2 = selectDataLog(txtPrdOrder.Text, Val(maxIncrementID()), "st2_check")
        valST4 = selectDataLog(txtPrdOrder.Text, Val(maxIncrementID()), "st4_check")

        valTestresults = selectDataLog(txtPrdOrder.Text, Val(maxIncrementID()), "final_result")

        If System.IO.File.Exists(varFile) = True Then
            File.AppendAllText(varFile, String.Format(txtPrdOrder.Text & ", " & txtPrdReference.Text & ", " & valST2 & ", " & valST4 & ", " & valTestresults & ",  " & selectDataLog(txtPrdOrder.Text, Val(maxIncrementID()), "start_time") & ", " & getUserName & vbCrLf, vbCrLf))
        Else
            File.Create(varFile).Dispose()
            File.AppendAllText(varFile, String.Format("Prd.Order, Reference, ST2 Check, ST4 Check, Final Result, Log Date, Log by" & vbCrLf, vbCrLf))
            File.AppendAllText(varFile, String.Format(txtPrdOrder.Text & ", " & txtPrdOrder.Text & ", " & valST2 & ", " & valST4 & ", " & valTestresults & ", " & selectDataLog(txtPrdOrder.Text, Val(maxIncrementID()), "start_time") & ", " & getUserName & vbCrLf, vbCrLf))
        End If

    End Sub

    Private Sub txtDetectSize_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDetectSize.TextChanged

        If txtLaunchSize.Text <> "" And Val(frmMapping.txtMW0.Text) = 1 Then
            If txtDetectSize.Text <> "" Then

                If txtDetectSize.Text = "ST2 Check OK" Then 'Mid(txtDetectSize.Text, 1, 1) & Mid(txtDetectSize.Text, 6, 1) = Mid(txtLaunchSize.Text, 1, 2) Then ' & txtLaunchVoltage.Text Then

                    txtDetectSize.BackColor = Color.LimeGreen

                    If Val(frmMapping.txtMW6.Text) = 1 Then
                        recordST2Pass()
                    End If

                Else

                    txtDetectSize.BackColor = Color.Red

                    If Val(frmMapping.txtMW6.Text) = 1 Then

                        txtFailQty.Text = Val(txtFailQty.Text) + 1

                        recordST2Fail()

                    End If
                End If
            Else
                txtDetectSize.BackColor = Color.White
            End If
        End If
    End Sub

    Private Sub btnReference_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReference.Click
        doSetDisplay("Reference")
        frmReference.doSetMode("READ")
        frmReference.doSetModeGroup("READ")
        frmReference.viewListReference("")
        frmReference.viewListFamily()
        frmReference.viewListDataLabel("")
        frmReference.viewListVision()

    End Sub

    Private Sub btnLogout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        Dim resultDelete As DialogResult = MessageBox.Show("Are you sure want to logout?", "User Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If resultDelete = DialogResult.Yes Then
            doSetDisplay("Login")
        End If
    End Sub

    Private Sub btnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
        frmAbout.ShowDialog()
    End Sub

    Private Sub tmrClock_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrClock.Tick
        Dim jam As String
        Dim menit As String
        Dim detik As String

        If Now.Hour.ToString.Length = 1 Then
            jam = "0" & Now.Hour
        Else
            jam = Now.Hour
        End If

        If Now.Minute.ToString.Length = 1 Then
            menit = "0" & Now.Minute
        Else
            menit = Now.Minute
        End If

        If Now.Second.ToString.Length = 1 Then
            detik = "0" & Now.Second
        Else
            detik = Now.Second
        End If

        lblClock.Text = jam & ":" & menit & ":" & detik
    End Sub

    Function RTLErrorCode(ByVal val As String) As String
        On Error Resume Next

        Dim strVal

        For a = 1 To val.Length
            strVal = strVal + Mid(val, 17 - a, 1)
        Next

        Return strVal

    End Function

    Private Sub btnDebug_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMapping.Click
        doSetDisplay("Debug")
    End Sub


    Private Sub btnViewWO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewWO.Click
        'frmWorkOrder.ShowDialog()
    End Sub

    Public Sub viewListWorkOrder(ByVal PONum As String)
        dgvWorkOrder.Rows.Clear()
        dgvWorkOrder.Columns.Clear()
        Dim strSQL As String
        Call connectServer()
        If PONum = "" Then
            strSQL = "SELECT * FROM tblWorkOrder"
        Else 'If stat = "CLOSE" Then
            strSQL = "SELECT * FROM tblWorkOrder WHERE prd_order = '" & PONum & "'"
        End If

        com = New MySqlCommand(strSQL, conn)
        Dim da As New MySqlDataAdapter(strSQL, conn)
        Dim ds As New DataSet
        'Dim reader As MySqlDataReader = com.ExecuteReader
        'Dim source As New BindingSource

        da.Fill(ds)
        da.Dispose()

        dgvWorkOrder.Columns.Add("Prd.Or #", "Prd.Or #")
        dgvWorkOrder.Columns.Add("Reference", "Reference")
        dgvWorkOrder.Columns.Add("Qty", "Qty")
        dgvWorkOrder.Columns.Add("Pass Qty", "Pass Qty")
        dgvWorkOrder.Columns.Add("Fail Qty", "Fail Qty")
        dgvWorkOrder.Columns.Add("Launch Date", "Launch Date")
        dgvWorkOrder.Columns.Add("Closed Date", "Closed Date")
        ' dgvWorkOrder.Columns.Add("Created By", "Created By")
        'dgvWorkOrder.Columns.Add("Closed Date", "Closed Date")
        'dgvWorkOrder.Columns.Add("Closed By", "Closed By")
        dgvWorkOrder.Columns.Add("Status", "Status")
        'dgvWorkOrder.Columns.Add("Clise", "Clise")

        Dim baris = ds.Tables(0).Rows
        Dim statBox
        For part = 0 To baris.Count - 1

            dgvWorkOrder.Rows.Add(baris(part).Item("prd_order").ToString, baris(part).Item("prd_reference").ToString, baris(part).Item("prd_qty").ToString, baris(part).Item("pass_qty").ToString, baris(part).Item("fail_qty").ToString, baris(part).Item("launch_date").ToString, baris(part).Item("close_date").ToString, baris(part).Item("close_date").ToString)

        Next                'If gdvPartData.Columns(0).HeaderText = "user_id" Then

    End Sub

    Sub closeWO()
        Try
            'queryDB("UPDATE tblWorkOrder SET status = 'close', close_date = '" & dateNowMysql() & "' WHERE prd_order = '" & txtPrdOrder.Text & "'")

            txtWOstatus.Text = "CLOSE"

            '            writePLC(0, 0)
        Catch EX As Exception
            MsgBox("closeWO : " & EX.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Function selectWIPTester(ByVal SONum As String) As Integer

        Dim selectWIP As String = readDataBase("SELECT MAX(wip) as wipx from tblLogTester where prd_order  = '" & SONum & "'", "wipx")

        If selectWIP = "NULL" Then
            Return 0
        Else
            Return Val(selectWIP)

        End If
    End Function

    Sub updateResult(ByVal valResult As String)
        If Val(frmMapping.txtMW0.Text) <> 0 Then

            If Val(frmMapping.txtMW0.Text) = 1 Then
                Try
                    'If valResult = "PASS" Then
                    '    queryDB("UPDATE tblWorkOrder SET pass_qty = pass_qty + 1 WHERE prd_order = '" & txtPrdOrder.Text & "'")
                    'ElseIf valResult = "FAIL" Then
                    '    queryDB("UPDATE tblWorkOrder SET fail_qty = fail_qty + 1 WHERE prd_order = '" & txtPrdOrder.Text & "'")
                    'End If

                    viewListWorkOrder("")

                    createFileLog()

                Catch ex As Exception
                    MsgBox("ERROR 1 : " & ex.Message, MsgBoxStyle.Critical)
                End Try
            End If

        End If
    End Sub

    Private Sub txtPassQty_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassQty.TextChanged
        If txtPassQty.Text <> "" Then
            If Val(txtPassQty.Text) <> 0 Then
                txtFPYPass.Text = Round((Val(txtPassQty.Text) / (Val(txtPassQty.Text) + Val(txtFailQty.Text))) * 100).ToString & "%"
                txtFPYFail.Text = Round((Val(txtFailQty.Text) / (Val(txtPassQty.Text) + Val(txtFailQty.Text))) * 100).ToString & "%"
                updateResult("PASS")
                If Val(txtPassQty.Text) >= Val(txtPrdQty.Text) Then
                    closeWO()
                End If
            Else
                txtFPYPass.Text = "0%"
            End If
        Else
            txtFPYPass.Text = ""
        End If

    End Sub

    Private Sub txtFailQty_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFailQty.TextChanged
        If txtFailQty.Text <> "" Then
            If Val(txtFailQty.Text) <> 0 Then
                txtFPYPass.Text = Round((Val(txtPassQty.Text) / (Val(txtPassQty.Text) + Val(txtFailQty.Text))) * 100).ToString & "%"
                txtFPYFail.Text = Round((Val(txtFailQty.Text) / (Val(txtPassQty.Text) + Val(txtFailQty.Text))) * 100).ToString & "%"
                updateResult("FAIL")
            Else
                txtFPYFail.Text = "0%"
            End If
        Else
            txtFPYFail.Text = ""
        End If


        'If Val(txtFailQty.Text) <> 0 Then
        '    updateResult("FAIL")
        'End If
    End Sub

    Private Sub btnUsers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUsers.Click
        doSetDisplay("User Management")
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        writePLC(0, 0)
        killLPPA()
        MsgBox("Sayonara!")
    End Sub

    Private Sub frmMain_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        writePLC(0, 0)
        killLPPA()
        MsgBox("Arigatou!")
    End Sub


    Private Sub tmrReadPLC_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrReadPLC.Tick
        frmMapping.txtMW0.Text = monautomate.mwLectureAutomate(0)

        txtMW2.Text = monautomate.mwLectureAutomate(2)

        frmMapping.txtMW4.Text = monautomate.mwLectureAutomate(4)

        frmMapping.txtMW21.Text = monautomate.mwLectureAutomate(21)

        Dim mw21 As Integer = frmMapping.txtMW21.Text

        Dim stringMW21 As String = Convert.ToString(mw21, 2).PadLeft(16, "0"c) '.Substring(1, 16)

        stringMW21 = RTLErrorCode(stringMW21)

        sensor1ST2 = Val(Mid(stringMW21, 1, 1))
        sensor2ST2 = Val(Mid(stringMW21, 2, 1))
        sensor3ST2 = Val(Mid(stringMW21, 3, 1))
        sensor4ST2 = Val(Mid(stringMW21, 4, 1))
        sensor5ST2 = Val(Mid(stringMW21, 5, 1))
        sensor6ST2 = Val(Mid(stringMW21, 6, 1))
        sensor7ST2 = Val(Mid(stringMW21, 7, 1))

        frmMapping.txtST2_1.Text = sensor1ST2
        frmMapping.txtST2_2.Text = sensor2ST2
        frmMapping.txtST2_3.Text = sensor3ST2
        frmMapping.txtST2_4.Text = sensor4ST2
        frmMapping.txtST2_5.Text = sensor5ST2
        frmMapping.txtST2_6.Text = sensor6ST2
        frmMapping.txtST2_7.Text = sensor7ST2

        frmMapping.txtMW2.Text = monautomate.mwLectureAutomate(2)
        frmMapping.txtMW3.Text = monautomate.mwLectureAutomate(3)
        frmMapping.txtMW6.Text = monautomate.mwLectureAutomate(6)
        frmMapping.txtMW1.Text = monautomate.mwLectureAutomate(1)

        frmMapping.txtMW28.Text = monautomate.mwLectureAutomate(28)

        frmMapping.txtMW80.Text = monautomate.mwLectureAutomate(80)

        frmMapping.txtMW29.Text = monautomate.mwLectureAutomate(29)
        frmMapping.txtMW31.Text = monautomate.mwLectureAutomate(31)
        frmMapping.txtMW32.Text = monautomate.mwLectureAutomate(32)
        frmMapping.txtMW33.Text = monautomate.mwLectureAutomate(33)
        frmMapping.txtMW19.Text = monautomate.mwLectureAutomate(18)

        txtMW33.Text = monautomate.mwLectureAutomate(33)
        txtMW98.Text = monautomate.mwLectureAutomate(98)
        frmMapping.txtMW50.Text = monautomate.mwLectureAutomate(50)
        frmMapping.txtMW50.Text = monautomate.mwLectureAutomate(50)

        frmMapping.txtMW56.Text = monautomate.mwLectureAutomate(56)
        frmMapping.txtMW57.Text = monautomate.mwLectureAutomate(57)

        'frmMapping.txtMW44.Text = monautomate.mwLectureAutomate(mw_job_backcam_status)
        'frmMapping.txtMW44.Text = monautomate.mwLectureAutomate(mw_job_frontcam_status)

        frmMapping.txt2MW5.Text = monautomate2.mwLectureAutomate(mw2_job_trigger)

        frmMapping.txtMW47.Text = monautomate.mwLectureAutomate(mw_frontcam_status)
        frmMapping.txtMW48.Text = monautomate.mwLectureAutomate(mw_backcam_status)


        If monautomate.mwLectureAutomate(mw_backcam_status) = 0 Or monautomate.mwLectureAutomate(mw_frontcam_status) = 0 Then
            'btnSetVision.Enabled = False
        Else
            'btnSetVision.Enabled = True
        End If

        If Val(txtJobID.Text) <> 27 Then
            If monautomate.mwLectureAutomate(mw_job_backcam_status) = 1 And monautomate.mwLectureAutomate(mw_job_frontcam_status) = 1 Then
                'btnSetVision.Visible = False
            Else
                If txtPrdQty.Text <> "" Then
                    'btnSetVision.Visible = True
                Else
                    'btnSetVision.Visible = False
                End If
            End If
        Else
            'btnSetVision.Enabled = False
        End If

        lblMW51.Text = monautomate.mwLectureAutomate(mw_job_backcam_status)
        lblMW52.Text = monautomate.mwLectureAutomate(mw_job_frontcam_status)


        If monautomate.mwLectureAutomate(mw_force_vision_result) = 1 Then
            indForceVision.Visible = True
        Else
            indForceVision.Visible = False
        End If

        If monautomate.mwLectureAutomate(mw_bypass_vision_check) = 1 Then
            indBypassSequence.Visible = False
        Else
            indBypassSequence.Visible = True
        End If


        frmMapping.txtMW51.Text = monautomate.mwLectureAutomate(mw_job_backcam_status)

        frmMapping.txtMW52.Text = monautomate.mwLectureAutomate(mw_job_frontcam_status)

        If monautomate.mwLectureAutomate(mw_vision_final_result) = 1 Then
            txtDetectVision.Text = "PASS"
        ElseIf monautomate.mwLectureAutomate(mw_vision_final_result) = 2 Then
            txtDetectVision.Text = "FAIL"
        Else
            txtDetectVision.Text = ""
        End If

    End Sub

    Private Sub btnDataLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDataLog.Click
        doSetDisplay("Data Log")
    End Sub

    Private Sub infStatMachine_BackColorChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles infStatMachine.BackColorChanged
        If infStatMachine.BackColor = Color.Gold Then
            infStatMachine.Text = "OFF"
        Else
            infStatMachine.Text = "ON"
        End If
    End Sub

    Private Sub srpBarcodeLabel_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles srpBarcodeLabel.DataReceived
        '     If Val(frmMapping.txtMW32.Text) = 3 Then
        If Val(txtMW33.Text) = 5 Then

            If txtPrdOrder.Text <> "" And txtPrdReference.Text <> "" And txtPrdQty.Text <> "" Then
                txtDetectVision.Text = Mid((srpBarcodeLabel.ReadLine), 1, 12)
            End If

        End If
        ' End If
    End Sub

    Private Sub txtDetectVision_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDetectVision.TextChanged
        If txtDetectVision.Text <> "" Then
            If txtDetectVision.Text = "PASS" Then
                txtDetectVision.BackColor = Color.LimeGreen
                recordST4Pass()
            Else
                txtDetectVision.BackColor = Color.Red
                txtFailQty.Text = Val(txtFailQty.Text) + 1
                recordST4Fail()
            End If
        Else

            txtDetectVision.BackColor = Color.White
        End If

    End Sub



    Private Sub tmrTriggerScan_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrTriggerScan.Tick
        If chkScan.Checked = False Then
            readscan()
        End If
    End Sub


    Private Sub txtReadScan_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReadScan.TextChanged
        If Len(txtReadScan.Text) = 0 Then
            tmrTriggerScan.Enabled = False
        ElseIf Len(txtReadScan.Text) = 1 Then
            tmrTriggerScan.Enabled = True
        End If
    End Sub

    Function rowError(ByVal descCode As String) As Integer
        For x = 0 To dgvErrorCode.Rows.Count - 1
            If dgvErrorCode.Rows(x).Cells(0).Value = descCode Then
                Return x
            End If
        Next
    End Function

    Private Sub txtMW2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW2.TextChanged
        If Val(txtMW2.Text) = 1 Then
            printLabel(labelCode)
            writePLC(3, 1)

        End If
    End Sub

    Private Sub txtMW32_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW32.TextChanged

        If Val(txtMW32.Text) = 1 Then
        ElseIf Val(txtMW32.Text) = 2 Then

        End If

    End Sub

    Private Sub txtMW33_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW33.TextChanged
        If Val(txtMW33.Text) = 1 Then
            '            txtDetectVision.Text = ""
            txtMW32.Text = "3"
        End If
    End Sub

    Private Sub txtMW98_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW98.TextChanged
        If Val(txtMW98.Text) = 1 Then
            txtMW2.Text = "3"
        End If
    End Sub

    Private Sub dgvErrorCode_RowsAdded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgvErrorCode.RowsAdded
        If dgvErrorCode.RowCount > 0 Then
            dgvErrorCode.Columns(0).HeaderText = "Error Code (" & dgvErrorCode.RowCount & ")"
        Else
            dgvErrorCode.Columns(0).HeaderText = "Error Code"
        End If
    End Sub

    Private Sub dgvErrorCode_RowsRemoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dgvErrorCode.RowsRemoved
        If dgvErrorCode.RowCount > 0 Then
            dgvErrorCode.Columns(0).HeaderText = "Error Code (" & dgvErrorCode.RowCount & ")"
        Else
            dgvErrorCode.Columns(0).HeaderText = "Error Code"
        End If
    End Sub

    Private Sub btnCodesoft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodesoft.Click
        frmCodesoft.ShowDialog()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim dialogBox As DialogResult = MessageBox.Show("Yakin ingin Print Manual?", "Printing", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If dialogBox = DialogResult.Yes Then
            printLabel(labelCode)
        End If
    End Sub

    Private Sub infStatMachine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles infStatMachine.Click
        deletePrintJob()
    End Sub

    Private Sub pnlRejectAlarm_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlRejectAlarm.Paint

    End Sub

    'Private Sub btnCheckingVisionResult_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckingVisionResult.CheckedChanged
    '    If btnCheckingVisionResult.Checked = True Then
    '        writePLC(mw_force_vision_result, 0)
    '    Else
    '        writePLC(mw_force_vision_result, 1)
    '    End If
    'End Sub

    Private Sub btnSetVision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        triggerJobVision()
    End Sub

    Private Sub btnSetVision_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If txtPrdQty.Text = "" Then
        '    btnSetVision.Visible = False
        '    Exit Sub
        'End If



        'If btnSetVision.Visible = True Then
        '    dgvErrorCode.Rows.Insert(0, "Load job vision gagal, tekan tombol SET VISION!", dateNowMysql)
        '    writePLC(0, 0)
        'Else

        '    If dgvErrorCode.Rows.Count > 0 Then
        '        dgvErrorCode.Rows.RemoveAt(rowError("Load job vision gagal, tekan tombol SET VISION!!")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
        '        writePLC(0, 1)
        '    End If
        'End If
    End Sub

    Private Sub btnBypass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBypass.Click
        frmVisionSetting.Show()
    End Sub

End Class
