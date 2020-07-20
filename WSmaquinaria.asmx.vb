Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters
Imports System.Web.Script.Services
Imports System.Data
Imports ABCmaquinaria
Imports System.Web.Script.Serialization

' Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WSmaquinaria
    Inherits System.Web.Services.WebService

    Public Class Resultado
        Public dato As String
        Public mensaje As String
    End Class

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getEquipos(termino As String) As String
        Dim odtEquipos As DataTable
        Dim oDB As New DB()
        odtEquipos = oDB.getEquipos(termino)

        Dim js As String = JsonConvert.SerializeObject(odtEquipos, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getObras(termino As String) As String
        Dim odtObras As DataTable
        Dim oDB As New DB()
        odtObras = oDB.getObras(termino)

        Dim js As String = JsonConvert.SerializeObject(odtObras, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getOperadores(termino As String) As String
        Dim odtOperadores As DataTable
        Dim oDB As New DB()
        odtOperadores = oDB.getOperadores(termino)

        Dim js As String = JsonConvert.SerializeObject(odtOperadores, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)>
    Public Function setUbicaciones(ByVal idEconomico As String, ByVal idOperador As String, ByVal idObra As String, ByVal fecha_alta As String,
                                   ByVal comentarios As String, ByVal idUsuario As String, ByVal odometro As String, ByVal sello As String,
                                   ByVal litros As String, ByVal horometro As String, ByVal ventana As String) As String
        Dim lsError As String = ""
        Dim oDB As New DB()
        Dim oResultado As New Resultado()

        If oDB.setUbicacion(idEconomico, idOperador, idObra, fecha_alta, comentarios, idUsuario, odometro, sello, litros, horometro, ventana, lsError) Then
            oResultado.dato = "S"
            oResultado.mensaje = ""
        Else
            oResultado.dato = "N"
            oResultado.mensaje = lsError
        End If

        Dim js As String = JsonConvert.SerializeObject(oResultado, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getAcceso(ByVal usuario As String, ByVal passw As String) As String
        Dim lsError As String = ""
        Dim lsCategoria As String = ""
        Dim oDB As New DB()
        Dim oResultado As New Resultado()

        If oDB.getAcceso(usuario, passw, lsCategoria) Then
            oResultado.dato = "S"
            oResultado.mensaje = ""
            Session("usuario") = usuario
            Session("categoria") = lsCategoria
        Else
            oResultado.dato = "N"
            oResultado.mensaje = "revisar Usario o contraseña"
        End If

        Dim js As String = JsonConvert.SerializeObject(oResultado, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getUbicaciones(ByVal fechaini As String, ByVal fechafin As String, ByVal equipo As String, ByVal obra As String, ByVal operador As String) As String
        Dim lsError As String = ""
        Dim oDB As New DB()
        Dim oResultado As New Resultado()
        Dim dtUbicaciones As New DataTable()

        dtUbicaciones = oDB.getUbicaciones(fechaini, fechafin, equipo, obra, operador)

        Dim js As String = JsonConvert.SerializeObject(dtUbicaciones, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getSession(categoriaAcceso As String) As String
        Dim lsError As String = ""

        Dim oResultado As New Resultado()

        If Session("usuario") Is Nothing Then
            oResultado.dato = "N"
        Else
            oResultado.dato = Session("categoria")
        End If

        Dim js As String = JsonConvert.SerializeObject(oResultado, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getConsultaListado(ByVal fecha As String) As String
        Dim lsError As String = ""
        Dim oDB As New DB()
        Dim oResultado As New Resultado()
        Dim dtUbicaciones As New DataTable()
        Dim usuario As String = Session("usuario")

        dtUbicaciones = oDB.getConsultaListado(fecha, usuario)

        Dim js As String = JsonConvert.SerializeObject(dtUbicaciones, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function setEliminaUbicacion(ByVal idEconomico As String, ByVal idOperador As String, ByVal idObra As String, ByVal fecha As String) As String
        Dim lsError As String = ""
        Dim oDB As New DB()
        Dim oResultado As New Resultado()
        Dim Usuario As String = Session("usuario")

        If oDB.setEliminaUbicacion(idEconomico, idOperador, idObra, fecha, Usuario, lsError) Then
            oResultado.dato = "S"
            oResultado.mensaje = ""
        Else
            oResultado.dato = "N"
            oResultado.mensaje = lsError
        End If

        Dim js As String = JsonConvert.SerializeObject(oResultado, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function setDiesel(ByVal idEconomico As String, ByVal idOperador As String, ByVal idObra As String, ByVal fecha_alta As String, ByVal Hodometro As String, ByVal Litros As String, ByVal idUsuario As String) As String
        Dim lsError As String = ""
        Dim oDB As New DB()
        Dim oResultado As New Resultado()

        If oDB.setDiesel(idEconomico, idOperador, idObra, fecha_alta, Hodometro, Litros, idUsuario, lsError) Then
            oResultado.dato = "S"
            oResultado.mensaje = ""
        Else
            oResultado.dato = "N"
            oResultado.mensaje = lsError
        End If

        Dim js As String = JsonConvert.SerializeObject(oResultado, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getListadoDiesel(ByVal fecha As String) As String
        Dim lsError As String = ""
        Dim oDB As New DB()
        Dim oResultado As New Resultado()
        Dim dtDiesel As New DataTable()
        Dim usuario As String = Session("usuario")

        dtDiesel = oDB.getListadoDiesel(fecha, usuario)

        Dim js As String = JsonConvert.SerializeObject(dtDiesel, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function setEliminaDiesel(ByVal idEconomico As String, ByVal idOperador As String, ByVal idObra As String, ByVal fecha As String) As String
        Dim lsError As String = ""
        Dim oDB As New DB()
        Dim oResultado As New Resultado()
        Dim Usuario As String = Session("usuario")

        If oDB.setEliminaDiesel(idEconomico, idOperador, idObra, fecha, Usuario, lsError) Then
            oResultado.dato = "S"
            oResultado.mensaje = ""
        Else
            oResultado.dato = "N"
            oResultado.mensaje = lsError
        End If

        Dim js As String = JsonConvert.SerializeObject(oResultado, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getConsultaPantalla() As String
        Dim lsError As String = ""
        Dim oDB As New DB()
        Dim dtPantalla As DataTable
        Dim Usuario As String = Session("usuario")
        Dim intPaginaActual As Integer

        If Session("intPaginaActual") = 0 Then
            intPaginaActual = 1
        Else
            intPaginaActual = Session("intPaginaActual")
            intPaginaActual = intPaginaActual + 1
        End If
        Session("intPaginaActual") = intPaginaActual
        

        Dim intPaginas As Integer

        dtPantalla = oDB.getConsutaPantalla(intPaginaActual)
        intPaginas = dtPantalla(0)(8)
        If dtPantalla.Rows.Count = 1 Then
            Session("intPaginaActual") = 0
        End If
        Session("intPaginas") = intPaginas
        If (Session("intPaginas") = intPaginaActual) Then
            Session("intPaginaActual") = 0
        End If

        dtPantalla.Columns.Remove("paginas")

        Dim js As String = JsonConvert.SerializeObject(dtPantalla, Newtonsoft.Json.Formatting.None)
        Return js

    End Function
End Class