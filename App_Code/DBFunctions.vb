Imports System.Data.SqlClient
Imports System.Data
Public Class DBFunctions


    Dim objConnection As SqlConnection
    Public Sub New()
        'Constructor, to connect to DB.
        ConnectToDatabase(True)
    End Sub
    Private Function ConnectToDatabase(ByVal blnConnect As Boolean) As Integer
        'blnConnect - If true then connect to database else disconnect.
        'Return Value - Returns 0 if successful, -1 if fails.
        Try
            If blnConnect = True Then
                objConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("LocalDB").ConnectionString.ToString())
                objConnection.Open()
            Else
                objConnection.Close()
                objConnection.Dispose()
            End If
            Return 0
        Catch e As Exception
            Return -1
        End Try
    End Function
    Public Function FillDataset(ByVal sql As String) As DataSet
        'Fills the dataset.
        Dim objAdapter = New SqlDataAdapter(sql, objConnection)
        Dim objDataSet = New DataSet
        objAdapter.Fill(objDataSet)
        objAdapter.Dispose()
        Return objDataSet
    End Function
    Public Function FillDataTable(ByVal sql As String) As DataTable
        'Fills the dataTabe.
        Dim objAdapter = New SqlDataAdapter(sql, objConnection)
        Dim objDataTable = New DataTable
        objAdapter.Fill(objDataTable)
        objAdapter.Dispose()
        Return objDataTable
    End Function
    Public Sub RunCommand(ByVal sql As String)
        'Not working properly.
        Dim objCommand As New SqlCommand()
        objCommand.Connection = objConnection
        objCommand.CommandText = sql
        objCommand.ExecuteNonQuery()
        objCommand.Dispose()
    End Sub
    Protected Overrides Sub Finalize()
        'Distructor, to disconnect from DB.
        ConnectToDatabase(False)
    End Sub
End Class
