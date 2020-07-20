Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class DB

    ' conexión string
    Dim conn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connMaquinaria").ConnectionString
    Dim DBConn As New SqlConnection(conn)
    Dim DBAdaptador As SqlDataAdapter
    Dim DBcomando As SqlCommand

    Public Function getEquipos(ByVal dato As String) As DataTable
        Dim odtEquipos As New DataTable()

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select idEconomico, Tipo from MAQUINARIA where (idEconomico like '%' + @dato + '%' or Tipo like '%' + @dato + '%') and estatus = 'A' ", DBConn)
            DBcomando.Parameters.AddWithValue("@dato", dato)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtEquipos)
            DBConn.Close()

            Return odtEquipos
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function getObras(ByVal dato As String) As DataTable
        Dim odtObras As New DataTable()

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select idObra, Nombre from obras where (idObra like '%' + @dato + '%' or Nombre like '%' + @dato + '%') and estatus = 'A' ", DBConn)
            DBcomando.Parameters.AddWithValue("@dato", dato)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtObras)
            DBConn.Close()

            Return odtObras
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function getOperadores(ByVal dato As String) As DataTable
        Dim odtOperadores As New DataTable()

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select idOperador, Nombre from operadores where (idOperador like '%' + @dato + '%' or Nombre like '%' + @dato + '%') and estatus = 'A' ", DBConn)
            DBcomando.Parameters.AddWithValue("@dato", dato)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtOperadores)
            DBConn.Close()

            Return odtOperadores
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function getUbicacion(ByVal idEconomico As String, ByVal idOperador As String, ByVal idObra As String, ByVal fecha_alta As String) As Boolean
        Dim odtUbicacion As New DataTable()
        Dim ArregloFecha() As String = fecha_alta.Split("/")
        Dim ldtFechaAlta As Date = New Date(ArregloFecha(2), ArregloFecha(1), ArregloFecha(0))

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select dato = count(1) from ubicacion where idEconomico = @idEconomico and idOperador = @idOperador and idObra = @idObra and fecha_alta = @fecha_alta", DBConn)
            DBcomando.Parameters.AddWithValue("@idEconomico", idEconomico)
            DBcomando.Parameters.AddWithValue("@idOperador", idOperador)
            DBcomando.Parameters.AddWithValue("@idObra", idObra)
            DBcomando.Parameters.AddWithValue("@fecha_alta", ldtFechaAlta)

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

    Public Function setUbicacion(ByVal idEconomico As String, ByVal idOperador As String, ByVal idObra As String, ByVal fecha_alta As String,
                                 ByVal comentarios As String, ByVal idUsuario As String, ByVal odometro As String, ByVal sello As String,
                                   ByVal litros As String, ByVal horometro As String, ByVal ventana As String, ByRef asError As String) As Boolean
        Dim odtOperadores As New DataTable()
        Dim ArregloFecha() As String = fecha_alta.Split("/")
        Dim ldtFechaAlta As Date = New Date(ArregloFecha(2), ArregloFecha(1), ArregloFecha(0))
        Dim ldtFechaActual = Date.Now()
        If odometro.Length = 0 Then
            odometro = "0"
        End If

        Try
            ' primero vamos a buscar si existe la ubicación.
            If Not getUbicacion(idEconomico, idOperador, idObra, fecha_alta) Then
                'asError = "La ubicación ya existe."
                'Return False
                ' MODIFICACION ESPECIAL. 20/07/2020
                DBcomando = New SqlCommand("insert into ubicacion(idEconomico, idOperador, idObra, fecha_alta, comentarios, idUsuario, fecha_ingreso, odometro, 
                                                                sello, litros, horometro, ventana)
                                        values(@idEconomico, @idOperador, @idObra, @fecha_alta, @comentarios, @idUsuario, @fecha_ingreso, @odometro,
                                                                 @sello, @litros, @horometro, @ventana)", DBConn)
            Else
                If ventana = "U" Then
                    DBcomando = New SqlCommand("update ubicacion 
                                               set comentarios = case when len(ISNULL(comentarios, '')) > 0 then comentarios + ' / ' + @comentarios else @comentarios end, 
                                                                  idUsuario = @idUsuario, 
                                                                    fecha_ingreso = @fecha_ingreso, odometro = @odometro, 
                                                                   ventana = @ventana
                                            where idEconomico = @idEconomico
                                                    and idOperador = @idOperador
                                                    and idObra = @idObra
                                                    and fecha_alta = @fecha_alta ", DBConn)
                Else
                    DBcomando = New SqlCommand("update ubicacion 
                                               set comentarios = case when len(ISNULL(comentarios, '')) > 0 then comentarios + ' / ' + @comentarios else @comentarios end, 
                                                                  idUsuario = @idUsuario, 
                                                                    fecha_ingreso = @fecha_ingreso,  
                                                                sello = @sello, litros = @litros, horometro = @horometro, ventana = @ventana
                                            where idEconomico = @idEconomico
                                                    and idOperador = @idOperador
                                                    and idObra = @idObra
                                                    and fecha_alta = @fecha_alta ", DBConn)

                End If

            End If

            DBConn.Open()

            DBcomando.Parameters.AddWithValue("@idEconomico", idEconomico)
            DBcomando.Parameters.AddWithValue("@idOperador", idOperador)
            DBcomando.Parameters.AddWithValue("@idObra", idObra)
            DBcomando.Parameters.AddWithValue("@fecha_alta", ldtFechaAlta)
            DBcomando.Parameters.AddWithValue("@comentarios", comentarios)
            DBcomando.Parameters.AddWithValue("@idUsuario", idUsuario)
            DBcomando.Parameters.AddWithValue("@fecha_ingreso", ldtFechaActual)
            DBcomando.Parameters.AddWithValue("@odometro", odometro)
            DBcomando.Parameters.AddWithValue("@sello", sello)
            DBcomando.Parameters.AddWithValue("@litros", litros)
            DBcomando.Parameters.AddWithValue("@horometro", horometro)
            DBcomando.Parameters.AddWithValue("@ventana", ventana)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtOperadores)
            DBConn.Close()

            Return True
        Catch ex As Exception
            asError = ex.Message.ToString()
            Return False
        End Try
    End Function

    Public Function getAcceso(ByVal usuario As String, ByVal passw As String, ByRef categoria As String) As Boolean
        Dim odtAcceso As New DataTable()

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select categoria from operadores where categoria in ('A', 'S') and idOperador = @usuario and passw = @passw and estatus = 'A' ", DBConn)
            DBcomando.Parameters.AddWithValue("@usuario", usuario)
            DBcomando.Parameters.AddWithValue("@passw", passw)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtAcceso)
            DBConn.Close()

            If odtAcceso.Rows.Count > 0 Then
                categoria = odtAcceso.Rows(0)("categoria")
                Return True
            Else
                categoria = ""
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function getUbicaciones(ByVal fechaini As String, ByVal fechafin As String, ByVal equipo As String, ByVal obra As String, ByVal operador As String) As DataTable
        Dim odtUbicaciones As New DataTable()
        Dim strFechaINI As String() = fechaini.Split("/")
        Dim strFechaFIN As String() = fechafin.Split("/")

        Dim ldtFechaIni As Date = New DateTime(strFechaINI(2), strFechaINI(1), strFechaINI(0))
        Dim ldtFechaFin As Date = New DateTime(strFechaFIN(2), strFechaFIN(1), strFechaFIN(0))

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select idEconomico, idOperador, idObra, fecha_alta = convert(VARCHAR, fecha_alta, 103), comentarios, idUsuario, odometro, 
                                                sello, litros, horometro, tipo = case when ventana = 'U' then 'Ubica' else 'Diesel' end
                                          from ubicacion 
                                        where (fecha_alta between @fechaini and @fechafin) 
                                                and (idObra = @obra or @obra = '') 
                                                 and (idOperador = @operador or @operador = '') 
                                                 and (idEconomico = @equipo or @equipo = '') ", DBConn)
            DBcomando.Parameters.AddWithValue("@fechaini", ldtFechaIni)
            DBcomando.Parameters.AddWithValue("@fechafin", ldtFechaFin)
            DBcomando.Parameters.AddWithValue("@equipo", equipo)
            DBcomando.Parameters.AddWithValue("@obra", obra)
            DBcomando.Parameters.AddWithValue("@operador", operador)

            DBAdaptador = New SqlDataAdapter(DBcomando)

            DBAdaptador.Fill(odtUbicaciones)
            DBConn.Close()

            Return odtUbicaciones

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function getConsultaListado(ByVal fechaListado As String, ByVal usuario As String) As DataTable
        Dim odtUbicaciones As New DataTable()
        Dim strFecha As String() = fechaListado.Split("/")

        Dim ldtFecha As Date = New DateTime(strFecha(2), strFecha(1), strFecha(0))

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select idEconomico, idOperador, idObra " & _
                                                " from ubicacion where fecha_alta = @fechaListado and idUsuario = @usuario ", DBConn)
            DBcomando.Parameters.AddWithValue("@fechaListado", ldtFecha)
            DBcomando.Parameters.AddWithValue("@usuario", usuario)

            DBAdaptador = New SqlDataAdapter(DBcomando)

            DBAdaptador.Fill(odtUbicaciones)
            DBConn.Close()

            Return odtUbicaciones

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function setEliminaUbicacion(ByVal idEconomico As String, ByVal idOperador As String, ByVal idObra As String, ByVal fecha As String, ByVal usuario As String, ByRef lsError As String) As Boolean
        Dim odtOperadores As New DataTable()
        Dim ArregloFecha() As String = fecha.Split("/")
        Dim ldtFechaAlta As Date = New Date(ArregloFecha(2), ArregloFecha(1), ArregloFecha(0))
        Dim ldtFechaActual = Date.Now()

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("delete from ubicacion where idEconomico = @idEconomico and idOperador = @idOperador and idObra = @idObra and fecha_alta = @fecha and idUsuario = @usuario", DBConn)
            DBcomando.Parameters.AddWithValue("@idEconomico", idEconomico)
            DBcomando.Parameters.AddWithValue("@idOperador", idOperador)
            DBcomando.Parameters.AddWithValue("@idObra", idObra)
            DBcomando.Parameters.AddWithValue("@fecha", ldtFechaAlta)
            DBcomando.Parameters.AddWithValue("@usuario", usuario)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtOperadores)
            DBConn.Close()

            Return True
        Catch ex As Exception
            lsError = ex.Message.ToString()
            Return False
        End Try
    End Function

    Public Function setDiesel(ByVal idEconomico As String, ByVal idOperador As String, ByVal idObra As String, ByVal fecha_alta As String, _
                                            ByVal Hodometro As String, ByVal Litros As String, ByVal idUsuario As String, ByRef asError As String) As Boolean
        Dim odtDiesel As New DataTable()
        Dim ArregloFechaTiempo As String() = fecha_alta.Split(" ")
        Dim ArregloFecha As String() = ArregloFechaTiempo(0).Split("/")
        Dim ArregloTiempo As String() = ArregloFechaTiempo(1).Split(":")
        Dim ldtFechaAlta As DateTime = New DateTime(ArregloFecha(2), ArregloFecha(1), ArregloFecha(0), ArregloTiempo(0), ArregloTiempo(1), 24)

        Dim ldtFechaActual = Date.Now()

        Try
            ' primero vamos a buscar si existe la ubicación.
            If getDiesel(idEconomico, idOperador, idObra, fecha_alta) Then
                asError = "El Diesel ya existe."
                Return False
            End If

            DBConn.Open()
            DBcomando = New SqlCommand("insert into diesel(idEconomico, idOperador, idObra, fecha_alta, hodometro, litros, idUsuario, fecha_ingreso)" & _
                                                "values(@idEconomico, @idOperador, @idObra, @fecha_alta, @Hodometro, @Litros, @idUsuario, @fecha_ingreso)", DBConn)
            DBcomando.Parameters.AddWithValue("@idEconomico", idEconomico)
            DBcomando.Parameters.AddWithValue("@idOperador", idOperador)
            DBcomando.Parameters.AddWithValue("@idObra", idObra)
            DBcomando.Parameters.AddWithValue("@fecha_alta", ldtFechaAlta)
            DBcomando.Parameters.AddWithValue("@Hodometro", Hodometro)
            DBcomando.Parameters.AddWithValue("@Litros", Litros)
            DBcomando.Parameters.AddWithValue("@idUsuario", idUsuario)
            DBcomando.Parameters.AddWithValue("@fecha_ingreso", ldtFechaActual)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtDiesel)
            DBConn.Close()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function getListadoDiesel(ByVal fechaListado As String, ByVal usuario As String) As DataTable
        Dim odtUbicaciones As New DataTable()
        Dim strFecha As String() = fechaListado.Split("/")

        Dim ldtFecha As DateTime = New DateTime(strFecha(2), strFecha(1), strFecha(0))

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select idEconomico, idOperador, idObra, hodometro, litros " & _
                                                " from diesel where convert(varchar(10), fecha_alta, 120) = convert(varchar(10), @ldtFecha, 120) and idUsuario = @usuario ", DBConn)
            DBcomando.Parameters.AddWithValue("@ldtFecha", ldtFecha)
            DBcomando.Parameters.AddWithValue("@usuario", usuario)

            DBAdaptador = New SqlDataAdapter(DBcomando)

            DBAdaptador.Fill(odtUbicaciones)
            DBConn.Close()

            Return odtUbicaciones

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function setEliminaDiesel(ByVal idEconomico As String, ByVal idOperador As String, ByVal idObra As String, ByVal fecha As String, ByVal usuario As String, ByRef lsError As String) As Boolean
        Dim odtDiesel As New DataTable()
        Dim ArregloFecha() As String = fecha.Split("/")
        Dim ldtFechaAlta As Date = New Date(ArregloFecha(2), ArregloFecha(1), ArregloFecha(0))
        Dim ldtFechaActual = Date.Now()

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("delete from diesel where idEconomico = @idEconomico and idOperador = @idOperador and idObra = @idObra and convert(varchar(10),fecha_alta, 120) = convert(varchar(10), @fecha, 120) and idUsuario = @usuario", DBConn)
            DBcomando.Parameters.AddWithValue("@idEconomico", idEconomico)
            DBcomando.Parameters.AddWithValue("@idOperador", idOperador)
            DBcomando.Parameters.AddWithValue("@idObra", idObra)
            DBcomando.Parameters.AddWithValue("@fecha", ldtFechaAlta)
            DBcomando.Parameters.AddWithValue("@usuario", usuario)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtDiesel)
            DBConn.Close()

            Return True
        Catch ex As Exception
            lsError = ex.Message.ToString()
            Return False
        End Try
    End Function

    Public Function getDiesel(ByVal idEconomico As String, ByVal idOperador As String, ByVal idObra As String, ByVal fecha_alta As String) As Boolean
        Dim odtDiesel As New DataTable()
        Dim ArregloFechaTiempo() As String = fecha_alta.Split(" ")
        Dim ArregloFecha() As String = ArregloFechaTiempo(0).Split("/")
        Dim ldtFechaAlta As Date = New Date(ArregloFecha(2), ArregloFecha(1), ArregloFecha(0))

        Try
            DBConn.Open()
            DBcomando = New SqlCommand("select dato = count(1) from diesel where idEconomico = @idEconomico and idOperador = @idOperador and idObra = @idObra and convert(varchar(10), fecha_alta, 120) = convert(varchar(10), @fecha_alta, 120)", DBConn)
            DBcomando.Parameters.AddWithValue("@idEconomico", idEconomico)
            DBcomando.Parameters.AddWithValue("@idOperador", idOperador)
            DBcomando.Parameters.AddWithValue("@idObra", idObra)
            DBcomando.Parameters.AddWithValue("@fecha_alta", ldtFechaAlta)

            DBAdaptador = New SqlDataAdapter(DBcomando)


            DBAdaptador.Fill(odtDiesel)
            DBConn.Close()

            If odtDiesel.Rows(0)("dato") > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function getConsutaPantalla(ByVal intPaginaActual As Integer) As DataTable
        Dim odtConsultaPantalla As New DataTable()
        Dim intMaxRenglones As Integer

        Try
            intMaxRenglones = getParametroNumerico("MaxRenglones")

            If DBConn.State = ConnectionState.Closed Then
                DBConn.Open()
            End If
            DBcomando = New SqlCommand("spConsultaUbicacionPaginado", DBConn)
            DBcomando.Parameters.AddWithValue("@intRenglones", intMaxRenglones)
            DBcomando.Parameters.AddWithValue("@intPagina", intPaginaActual)

            DBcomando.CommandType = CommandType.StoredProcedure
            DBAdaptador = New SqlDataAdapter(DBcomando)

            DBAdaptador.Fill(odtConsultaPantalla)
            DBConn.Close()

            Return odtConsultaPantalla
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function getParametroNumerico(ByVal strNombre As String) As Integer
        Dim odtConsultaParametro As New DataTable()
        Dim intValor As Integer = 0

        Try
            If DBConn.State = ConnectionState.Closed Then
                DBConn.Open()
            End If
            DBcomando = New SqlCommand("select valor from parametros where nombre = @strNombre", DBConn)
            DBcomando.Parameters.AddWithValue("@strNombre", strNombre)

            DBcomando.CommandType = CommandType.Text
            DBAdaptador = New SqlDataAdapter(DBcomando)

            DBAdaptador.Fill(odtConsultaParametro)
            DBConn.Close()

            If odtConsultaParametro.Rows.Count > 0 Then
                intValor = odtConsultaParametro(0)(0)
            End If

            Return intValor
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class
