Imports MySql.Data.MySqlClient.MySqlConnection
Imports MySql.Data.MySqlClient

Module Connection
    Public gDocGroup As LabelManager2.Document
    Public gLabel As LabelManager2.Application

    Public conn As New MySqlConnection
    Public reader As MySqlDataReader
    Public com As MySqlCommand

    Public Sub connectServer()
        conn.Close()
        Dim mystring As String = "server='127.0.0.1'; port='3306'; user='root'; pwd='Pass1234'; database='DB_TeSySD_Labeling';"
        Try
            conn.ConnectionString = mystring
            conn.Open()
        Catch ex As Exception
            MsgBox("ERROR : KOMPUTER BELUM READY, TUNGGU 10 HINGGA 20 DETIK KEMUDIAN JALANKAN LAGI!", MsgBoxStyle.Critical)
            End
        End Try
    End Sub

End Module