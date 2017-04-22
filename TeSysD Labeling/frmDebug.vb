Public Class frmMapping


    Private Sub txtMW0_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW0.TextChanged
        If Val(txtMW0.Text) = 1 Then
            frmMain.infStatMachine.BackColor = Color.LimeGreen
            frmMain.writePLC(2, 0)

            frmMain.writePLC(3, 0)

        Else
            frmMain.infStatMachine.BackColor = Color.Gold
            '            txtMW1.Text = "0"
        End If

    End Sub

    Private Sub txtMW2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW2.TextChanged
        If Val(txtMW2.Text) = 0 Then

        End If
    End Sub

    Private Sub txtMW4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW4.TextChanged
        If Val(txtMW4.Text) <> 0 Then
            If Val(txtMW4.Text) = 1 Then
                frmMain.startTime = frmMain.dateNowMysql
            ElseIf Val(txtMW4.Text) = 2 Then
                frmMain.endTime = frmMain.dateNowMysql

                'f    frmMain.updateEndTime()
            End If
        End If

    End Sub

    Private Sub txtMW28_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28.TextChanged
        Dim mw28 As Integer = Val(txtMW28.Text)

        Dim stringMW28 As String = Convert.ToString(mw28, 2).PadLeft(16, "0"c) '.Substring(1, 16)

        stringMW28 = frmMain.RTLErrorCode(stringMW28)


        txtMW28_1.Text = Val(Mid(stringMW28, 1, 1))
        txtMW28_2.Text = Val(Mid(stringMW28, 2, 1))
        txtMW28_3.Text = Val(Mid(stringMW28, 3, 1))
        txtMW28_4.Text = Val(Mid(stringMW28, 4, 1))
        txtMW28_5.Text = Val(Mid(stringMW28, 5, 1))
        txtMW28_6.Text = Val(Mid(stringMW28, 6, 1))
        txtMW28_7.Text = Val(Mid(stringMW28, 7, 1))
        txtMW28_8.Text = Val(Mid(stringMW28, 8, 1))

        txtMW28_9.Text = Val(Mid(stringMW28, 9, 1))
        txtMW28_10.Text = Val(Mid(stringMW28, 10, 1))
        txtMW28_11.Text = Val(Mid(stringMW28, 11, 1))
        txtMW28_12.Text = Val(Mid(stringMW28, 12, 1))
        txtMW28_13.Text = Val(Mid(stringMW28, 13, 1))
        txtMW28_14.Text = Val(Mid(stringMW28, 14, 1))
        txtMW28_15.Text = Val(Mid(stringMW28, 15, 1))
        txtMW28_16.Text = Val(Mid(stringMW28, 16, 1))

    End Sub

    '############################################### ERROR CODE ##########################################################

    Private Sub txtMW28_1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_1.TextChanged
        If Val(txtMW28_1.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Emergency Mode", frmMain.dateNowMysql)

            frmMain.writePLC(2, 0)

        ElseIf Val(txtMW28_1.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Emergency Mode")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_2.TextChanged
        If Val(txtMW28_2.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Reset Machine Before Start", frmMain.dateNowMysql)
            frmMain.writePLC(2, 0)

        ElseIf Val(txtMW28_2.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Reset Machine Before Start")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_3.TextChanged
        If Val(txtMW28_3.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Cylinder 1 Error", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_3.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Cylinder 1 Error")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_4.TextChanged
        If Val(txtMW28_4.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Cylinder 2 Error", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_4.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Cylinder 2 Error")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_5.TextChanged
        If Val(txtMW28_5.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Cylinder 3 Error", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_5.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Cylinder 3 Error")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_6.TextChanged
        If Val(txtMW28_6.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Cylinder 4 Error", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_6.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Cylinder 4 Error")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_7.TextChanged
        If Val(txtMW28_7.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Cylinder 5 Error", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_7.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Cylinder 5 Error")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_8_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_8.TextChanged
        If Val(txtMW28_8.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Cylinder 6 Error", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_8.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Cylinder 6 Error")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_9_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_9.TextChanged
        If Val(txtMW28_9.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Cylinder 7 Error", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_9.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Cylinder 7 Error")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_10_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_10.TextChanged
        If Val(txtMW28_10.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "The air supply is not enough", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_10.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("The air supply is not enough")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_11_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_11.TextChanged
        If Val(txtMW28_11.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Motor Linear not Ready", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_11.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Motor Linear not Ready")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_12_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_12.TextChanged
        If Val(txtMW28_12.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Door Open", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_12.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Door Open")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_13_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_13.TextChanged
        If Val(txtMW28_13.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Printer General Error", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_13.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Printer General Error")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_14_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_14.TextChanged
        If Val(txtMW28_14.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Printer / Applicator not ready", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_14.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Printer / Applicator not ready")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_15_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_15.TextChanged
        If Val(txtMW28_15.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Paper / ribbon is end", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_15.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Paper / ribbon is end")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW28_16_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW28_16.TextChanged
        If Val(txtMW28_16.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Belum ada", frmMain.dateNowMysql)
        ElseIf Val(txtMW28_16.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Belum ada")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW29_01_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW29_01.TextChanged
        If Val(txtMW29_01.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Produk stack di conveyor!", frmMain.dateNowMysql)
        ElseIf Val(txtMW29_01.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Produk stack di conveyor!")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW29_02_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW29_02.TextChanged
        If Val(txtMW29_02.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Reject bin is Full!", frmMain.dateNowMysql)
        ElseIf Val(txtMW29_02.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Reject bin is Full!")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW29_03_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW29_03.TextChanged
        If Val(txtMW29_03.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Please troubleshoot device printer in PC!", frmMain.dateNowMysql)
        ElseIf Val(txtMW29_03.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Please troubleshoot device printer in PC!")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW29_04_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW29_04.TextChanged
        If Val(txtMW29_04.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Reject 3x!", frmMain.dateNowMysql)
        ElseIf Val(txtMW29_04.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Reject 3x!")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW29_08_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW29_08.TextChanged
        If Val(txtMW29_08.Text) = 1 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Tekan RESET untuk melanjutkan proses!", frmMain.dateNowMysql)
        ElseIf Val(txtMW29_08.Text) = 0 Then
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Tekan RESET untuk melanjutkan proses!")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub




    '############################################### ERROR CODE ##########################################################

    Private Sub txtMW31_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW31.TextChanged
        If Val(txtMW31.Text) = 1 Then
        End If
    End Sub

    Private Sub txtMW57_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles txtMW57.TextChanged
        frmMain.mw57.Text = txtMW57.Text
        If Val(txtMW0.Text) = 1 Then

            If (Val(txtMW57.Text) = 1 And Val(txtMW56.Text) <> 1) Or (Val(txtMW57.Text) = 2 And Val(txtMW56.Text) <> 1) Then
                frmMain.txtDetectVision.BackColor = Color.Red
                If Val(txtMW6.Text) = 1 Then

                    frmMain.txtFailQty.Text = Val(frmMain.txtFailQty.Text) + 1
                    frmMain.recordST4Fail()
                End If
            End If

        End If

    End Sub

    Private Sub txtMW56_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW56.TextChanged

        frmMain.mw56.Text = txtMW56.Text

        If Val(txtMW0.Text) = 1 Then
            If Val(txtMW56.Text) = 1 Then
                frmMain.txtDetectVision.BackColor = Color.LimeGreen

                If Val(txtMW6.Text) = 1 Then
                    frmMain.txtPassQty.Text = Val(frmMain.txtPassQty.Text) + 1
                    frmMain.recordST4Pass()
                End If
            ElseIf Val(txtMW56.Text) = 2 Then
                frmMain.txtDetectVision.BackColor = Color.Red
                If Val(txtMW6.Text) = 1 Then
                    frmMain.txtFailQty.Text = Val(frmMain.txtFailQty.Text) + 1
                    frmMain.recordST4Fail()
                End If
            End If
        End If
    End Sub

    Private Sub txtMW3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW3.TextChanged
        If Val(txtMW3.Text) = 1 Then
            frmMain.writePLC(2, 0)

        End If
    End Sub

    Private Sub txtMW32_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW32.TextChanged
        frmMain.mw32.Text = txtMW32.Text
    End Sub



    Private Sub txtMW29_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW29.TextChanged
        Dim mw29 As Integer = Val(txtMW29.Text)

        Dim stringMW29 As String = Convert.ToString(mw29, 2).PadLeft(16, "0"c) '.Substring(1, 16)

        stringMW29 = frmMain.RTLErrorCode(stringMW29)


        txtMW29_01.Text = Val(Mid(stringMW29, 1, 1))
        txtMW29_02.Text = Val(Mid(stringMW29, 2, 1))
        txtMW29_03.Text = Val(Mid(stringMW29, 3, 1))
        txtMW29_04.Text = Val(Mid(stringMW29, 4, 1))
        txtMW29_08.Text = Val(Mid(stringMW29, 8, 1))


    End Sub

    Private Sub txtMW19_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW19.TextChanged

        If Val(txtMW19.Text) = 1 Then
            frmMain.txtDetectSize.Text = "ST2 Check OK"
        ElseIf Val(txtMW19.Text) = 2 Then
            frmMain.txtDetectSize.Text = "ST2 Check NG"
        Else
            frmMain.txtDetectSize.Text = ""

        End If

    End Sub

    Private Sub tmrReadPLC_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrReadPLC.Tick

    End Sub

    Private Sub txtMW44_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW44.TextChanged
    End Sub

    Private Sub TextBox55_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox55.TextChanged

    End Sub

    Private Sub txtMW47_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW47.TextChanged
        If Val(txtMW47.Text) = 0 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Insight Vision Front Cam is Not Ready!", frmMain.dateNowMysql)
        Else
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Insight Vision Front Cam is Not Ready!")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW48_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW48.TextChanged
        If Val(txtMW48.Text) = 0 Then
            frmMain.dgvErrorCode.Rows.Insert(0, "Insight Vision Back Cam is Not Ready!", frmMain.dateNowMysql)
        Else
            If frmMain.dgvErrorCode.Rows.Count > 0 Then
                frmMain.dgvErrorCode.Rows.RemoveAt(frmMain.rowError("Insight Vision Back Cam is Not Ready!")) '(0, "Cyl2 Rotate Stopper Stack", dateNowMysql)
            End If
        End If
    End Sub

    Private Sub txtMW51_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMW51.TextChanged
    End Sub
End Class