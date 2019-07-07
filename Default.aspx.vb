Imports System.Collections

Imports System.IO
Imports System.Data.SqlClient
Imports System.Data

Partial Class _Default
    Inherits System.Web.UI.Page
    Dim objDB As New DBFunctions()
    Dim ds As New DataSet()
    Dim sql As String
    Dim lstAsset As String = ""
    Dim TransType As Integer
    Dim arr()
    Dim dt As DataTable
    Dim counter As Integer
    Public strAlert As String = ""
    Dim con As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim con1 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim con2 As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("LocalDb").ConnectionString)
    Dim cmd1 As SqlCommand
    Dim cmd2 As SqlCommand
    Dim cmd3 As SqlCommand
    Dim rdr1 As SqlDataReader
    Dim rdr2 As SqlDataReader
    Dim rdr3 As SqlDataReader
    Dim sqladr As SqlDataAdapter
    Dim dtable As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
      


       
    End Sub
    
    
    
End Class
