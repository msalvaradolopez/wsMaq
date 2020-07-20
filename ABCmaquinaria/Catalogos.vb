Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class Catalogos
    ' conexión string
    Dim conn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connMaquinaria").ConnectionString
    Dim DBConn As New SqlConnection(conn)
    Dim DBAdaptador As SqlDataAdapter
    Dim DBcomando As SqlCommand

    Public Function setMaquinaria(ByVal Nuevo As String, ByVal idEconomico As String, ByVal Tipo As String, ByVal estatus As String, ByRef asError As String) As Boolean
        Dim odtEquipos As New DataTable()
        Dim ldtFechaActual = Date.Now()
        Dim sql As String = ""

        Try
            If Nuevo = "S" Then
                If getValidaMaquinaria(idEconomico) Then
                    asError = "El Equipo ya existe."
                    Return False
                End If
                sql = "insert maquinaria(ideconomico, tipo, estatus, fecha_alta) values(@idEconomico, @Tipo, @estatus, @fecha_alta)"
            Else
                sql = "update maquinaria set tipo = @Tipo, estatus = @estatus where idEconomico = @idEconomico"
            End If

            DBConn.Open()
            DBcomando = New SqlCommand(sql, DBConn)
            DBcomando.Parameters.AddWithValue("@idEconomico", idEconomico)
            DBcomando.Parameters.AddWithValue("@Tipo", Tipo)
            DBcomando.Parameters.AddWithValue("@estatus", estatus)
            DBcomando.Parameters.AddWithValue("@fecha_alta", ldtFechaActual)


            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtEquipos)
            DBConn.Close()

            Return True
        Catch ex As Exception
            asError = ex.Message.ToString()

            Return False
        End Try
    End Function

    Public Function getValidaMaquinaria(ByVal idEconomico As String) As Boolean
        Dim odtUbicacion As New DataTable()
        
        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select dato = count(1) from maquinaria where idEconomico = @idEconomico", DBConn)
            DBcomando.Parameters.AddWithValue("@idEconomico", idEconomico)
           
            DBAdaptador = New SqlDataAdapter(DBcomando)

            DBAdaptador.Fill(odtUbicacion)
            DBConn.Close()

            If odtUbicacion.Rows(0)("dato") > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function getMaquinaria() As DataTable
        Dim odtEquipos As New DataTable()
        Dim ldtFechaActual = Date.Now()

        Try

            DBConn.Open()
            DBcomando = New SqlCommand("select idEconomico, Tipo, Estatus = case when estatus = 'A' then 'Activo' else 'Baja' end, fecha_alta from maquinaria order by 1", DBConn)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtEquipos)
            DBConn.Close()

            Return odtEquipos
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function getMaquinariaDato(ByVal NoEquipo As String) As DataTable
        Dim odtEquipos As New DataTable()

        Try

            DBConn.Open()
            DBcomando = New SqlCommand("select idEconomico, Tipo, estatus from maquinaria where idEconomico = @NoEquipo order by 1", DBConn)
            DBcomando.Parameters.AddWithValue("@NoEquipo", NoEquipo)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtEquipos)
            DBConn.Close()

            Return odtEquipos
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Public Function getObras() As DataTable
        Dim odtObras As New DataTable()
        Dim ldtFechaActual = Date.Now()

        Try

            DBConn.Open()
            DBcomando = New SqlCommand("select idObra, Nombre, Estatus = case when estatus = 'A' then 'Activo' else 'Baja' end, fecha_alta from obras order by 1", DBConn)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtObras)
            DBConn.Close()

            Return odtObras
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function getObraDato(idObra As String) As DataTable
        Dim odtObras As New DataTable()
        Dim ldtFechaActual = Date.Now()

        Try

            DBConn.Open()
            DBcomando = New SqlCommand("select idObra, Nombre, estatus from obras where idObra = @idObra order by 1", DBConn)
            DBcomando.Parameters.AddWithValue("@idObra", idObra)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtObras)
            DBConn.Close()

            Return odtObras
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Public Function setObra(ByVal Nuevo As String, ByVal idObra As String, ByVal Nombre As String, ByVal estatus As String, ByRef asError As String) As Boolean
        Dim odtObra As New DataTable()
        Dim ldtFechaActual = Date.Now()
        Dim sql As String = ""

        idObra = idObra.ToUpper()
        Try

            If Nuevo = "S" Then
                If getValidaObra(idObra) Then
                    asError = "La Obra ya existe."
                    Return False
                End If
                sql = "insert obras(idObra, Nombre, estatus, fecha_alta) values(@idObra, @Nombre, @estatus, @fecha_alta)"
            Else
                sql = "update obras set Nombre = @Nombre, estatus = @estatus where idObra = @idObra"
            End If

            DBConn.Open()
            DBcomando = New SqlCommand(sql, DBConn)
            DBcomando.Parameters.AddWithValue("@idObra", idObra)
            DBcomando.Parameters.AddWithValue("@Nombre", Nombre)
            DBcomando.Parameters.AddWithValue("@estatus", estatus)
            DBcomando.Parameters.AddWithValue("@fecha_alta", ldtFechaActual)

            DBAdaptador = New SqlDataAdapter(DBcomando)

            DBAdaptador.Fill(odtObra)
            DBConn.Close()

            Return True
        Catch ex As Exception
            asError = ex.Message.ToString()

            Return False
        End Try
    End Function

    Public Function getValidaObra(ByVal idObra As String) As Boolean
        Dim odtObra As New DataTable()

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select dato = count(1) from obras where idObra = @idObra", DBConn)
            DBcomando.Parameters.AddWithValue("@idObra", idObra)

            DBAdaptador = New SqlDataAdapter(DBcomando)

            DBAdaptador.Fill(odtObra)
            DBConn.Close()

            If odtObra.Rows(0)("dato") > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function getOperadores() As DataTable
        Dim odtOperadores As New DataTable()
        Dim ldtFechaActual = Date.Now()

        Try

            DBConn.Open()
            DBcomando = New SqlCommand("select idOperador, Nombre, categoria = case when categoria = 'A' then 'Administrativo' when categoria = 'S' then 'Supervisor' when categoria = 'O' then 'Operador'  end ,Estatus = case when estatus = 'A' then 'Activo' else 'Baja' end, fecha_alta from operadores order by 1", DBConn)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtOperadores)
            DBConn.Close()

            Return odtOperadores
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function getOperadorDato(ByVal idOperador As String) As DataTable
        Dim odtOperadores As New DataTable()
        Dim ldtFechaActual = Date.Now()

        Try

            DBConn.Open()
            DBcomando = New SqlCommand("select idOperador, Nombre, categoria, estatus, passw from operadores where idOperador = @idOperador order by 1", DBConn)
            DBcomando.Parameters.AddWithValue("@idOperador", idOperador)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtOperadores)
            DBConn.Close()

            Return odtOperadores
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Public Function setOperador(ByVal Nuevo As String, ByVal idOperador As String, ByVal Nombre As String, ByVal estatus As String, ByVal categoria As String, ByVal passw As String, ByRef asError As String) As Boolean
        Dim odtOperadores As New DataTable()
        Dim ldtFechaActual = Date.Now()
        Dim sql As String = ""

        idOperador = idOperador.ToUpper()
        Nombre = Nombre.ToUpper()
        Try

            If Nuevo = "S" Then
                If getValidaOperador(idOperador) Then
                    asError = "El Operador ya existe."
                    Return False
                End If
                sql = "insert operadores(idOperador, Nombre, estatus, categoria,fecha_alta, passw) values(@idOperador, @Nombre, @estatus, @categoria, @fecha_alta, @passw)"
            Else
                sql = "update operadores set Nombre = @Nombre, estatus = @estatus, categoria = @categoria, passw = @passw where idOperador = @idOperador"
            End If

            DBConn.Open()
            DBcomando = New SqlCommand(sql, DBConn)
            DBcomando.Parameters.AddWithValue("@idOperador", idOperador)
            DBcomando.Parameters.AddWithValue("@Nombre", Nombre)
            DBcomando.Parameters.AddWithValue("@estatus", estatus)
            DBcomando.Parameters.AddWithValue("@fecha_alta", ldtFechaActual)
            DBcomando.Parameters.AddWithValue("@categoria", categoria)
            DBcomando.Parameters.AddWithValue("@passw", passw)


            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtOperadores)
            DBConn.Close()

            Return True
        Catch ex As Exception
            asError = ex.Message.ToString()

            Return False
        End Try
    End Function

    Public Function getValidaOperador(ByVal idOperador As String) As Boolean
        Dim odtOperador As New DataTable()

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select dato = count(1) from operadores where idOperador = @idOperador", DBConn)
            DBcomando.Parameters.AddWithValue("@idOperador", idOperador)

            DBAdaptador = New SqlDataAdapter(DBcomando)

            DBAdaptador.Fill(odtOperador)
            DBConn.Close()

            If odtOperador.Rows(0)("dato") > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
