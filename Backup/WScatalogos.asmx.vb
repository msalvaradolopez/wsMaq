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
 Public Class WScatalogos
    Inherits System.Web.Services.WebService

    Public Class Resultado
        Public dato As String
        Public mensaje As String
    End Class

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function setMaquinaria(Nuevo As String, idEconomico As String, Tipo As String, ByVal Estatus As String) As String
        Dim lsError As String = ""
        Dim oResultado As New Resultado()

        Dim oCatalogos As New Catalogos()

        oResultado.dato = "S"
        If Not oCatalogos.setMaquinaria(Nuevo, idEconomico, Tipo, Estatus, lsError) Then
            oResultado.dato = "N"
            oResultado.mensaje = lsError
        End If

        Dim js As String = JsonConvert.SerializeObject(oResultado, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getMaquinaria() As String
        Dim lsError As String = ""
        Dim oResultado As New Resultado()
        Dim odtMaquinaria As New DataTable()

        Dim oCatalogos As New Catalogos()

        odtMaquinaria = oCatalogos.getMaquinaria()

        Dim js As String = JsonConvert.SerializeObject(odtMaquinaria, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getMaquinariaDato(NoEquipo) As String
        Dim lsError As String = ""
        Dim oResultado As New Resultado()
        Dim odtMaquinaria As New DataTable()

        Dim oCatalogos As New Catalogos()

        odtMaquinaria = oCatalogos.getMaquinariaDato(NoEquipo)

        Dim js As String = JsonConvert.SerializeObject(odtMaquinaria, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getObras() As String
        Dim lsError As String = ""
        Dim oResultado As New Resultado()
        Dim odtObras As New DataTable()

        Dim oCatalogos As New Catalogos()

        odtObras = oCatalogos.getObras()

        Dim js As String = JsonConvert.SerializeObject(odtObras, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getObraDato(idObra As String) As String
        Dim lsError As String = ""
        Dim oResultado As New Resultado()
        Dim odtObras As New DataTable()

        Dim oCatalogos As New Catalogos()

        odtObras = oCatalogos.getObraDato(idObra)

        Dim js As String = JsonConvert.SerializeObject(odtObras, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function setObra(Nuevo As String, idObra As String, Nombre As String, estatus As String) As String
        Dim lsError As String = ""
        Dim oResultado As New Resultado()

        Dim oCatalogos As New Catalogos()

        oResultado.dato = "S"
        If Not oCatalogos.setObra(Nuevo, idObra, Nombre, estatus, lsError) Then
            oResultado.dato = "N"
            oResultado.mensaje = lsError
        End If

        Dim js As String = JsonConvert.SerializeObject(oResultado, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getOperadores() As String
        Dim lsError As String = ""
        Dim oResultado As New Resultado()
        Dim odtOperadores As New DataTable()

        Dim oCatalogos As New Catalogos()

        odtOperadores = oCatalogos.getOperadores()

        Dim js As String = JsonConvert.SerializeObject(odtOperadores, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function getOperadorDato(idOPerador As String) As String
        Dim lsError As String = ""
        Dim oResultado As New Resultado()
        Dim odtOperadores As New DataTable()

        Dim oCatalogos As New Catalogos()

        odtOperadores = oCatalogos.getOperadorDato(idOPerador)

        Dim js As String = JsonConvert.SerializeObject(odtOperadores, Newtonsoft.Json.Formatting.None)
        Return js

    End Function
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)> <WebMethod(True)> _
    Public Function setOperador(Nuevo As String, idOperador As String, Nombre As String, categoria As String, passw As String, estatus As String) As String
        Dim lsError As String = ""
        Dim oResultado As New Resultado()

        Dim oCatalogos As New Catalogos()

        oResultado.dato = "S"
        If Not oCatalogos.setOperador(Nuevo, idOperador, Nombre, estatus, categoria, passw, lsError) Then
            oResultado.dato = "N"
            oResultado.mensaje = lsError
        End If

        Dim js As String = JsonConvert.SerializeObject(oResultado, Newtonsoft.Json.Formatting.None)
        Return js

    End Function

End Class