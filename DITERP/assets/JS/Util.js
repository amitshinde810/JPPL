//Util.js http://www.Buosi.org

/*
Public Class TipoAviso
    Public Const Info As String = "AvisoInfo"
    Public Const Ok As String = "AvisoOk"
    Public Const Aviso As String = "AvisoAviso"
    Public Const Erro As String = "AvisoErro"
End Class
*/


var TipoAviso = {"Info": "AvisoInfo",
                 "Ok": "AvisoOk",
                 "Aviso": "AvisoAviso",
                 "Erro": "AvisoErro"};


function VerificaNumerico(NomeText, NomeDivAlerta)
{

    try
    {

        var valTextbox;
        var TextoAlerta;
        var bMostraErro = false;

        TextoAlerta = $(NomeText).attr('MsgObrigatorio');

        if ((typeof TextoAlerta === 'undefined') || (null == TextoAlerta) || (TextoAlerta == ""))
        {
            TextoAlerta = "Required Field!";
        }

        valTextbox = $(NomeText).val();

        //Verifica se é numérico
        if (isNaN(valTextbox) == true)
        {
            TextoAlerta = "Enter the field '" + TextoAlerta + "' with a valid number!";
            bMostraErro = true;
        }

        //Verifica se digitou algo numérico
        //if ((typeof valTextbox === 'undefined') || (null == valTextbox) || (valTextbox == 0))
        if ((typeof valTextbox === 'undefined') || (null == valTextbox))
        {
            TextoAlerta = "The field '" + TextoAlerta + "' is required!";
            bMostraErro = true;
        }


        if (bMostraErro == true)
        {
            MostrarMensagem(NomeDivAlerta, TextoAlerta, TipoAviso.Aviso);
            $(NomeText).focus().select();
            return false;
        }

        return true;

    }
    catch (err)
    {
        alert('Please check Number: ' + err.description);
        return false;
    }

}


function VerificaObrigatorio(NomeText, NomeDivAlerta)
{

    try
    {

        var valTextbox;
        var TextoAlerta;

        TextoAlerta = $(NomeText).attr('MsgObrigatorio');

        if ((typeof TextoAlerta === 'undefined') || (null == TextoAlerta) || (TextoAlerta == ""))
        {
            TextoAlerta = "Please insert Text!";
        }
        else
        {
            TextoAlerta = "The Field '" + TextoAlerta + "' is Required";
        }

        valTextbox = $(NomeText).val();


        //Verifica se digitou algo numérico
        if ((typeof valTextbox === 'undefined') || (null == valTextbox) || (valTextbox == ''))
        {
            MostrarMensagem(NomeDivAlerta, TextoAlerta, TipoAviso.Aviso);
            $(NomeText).focus().select();
            return false;
        }

        return true;

    }
    catch (err)
    {
        alert('Please check Text: ' + err.description);
        return false;
    }

}



function VerificaValorCombo(NomeCombo, NomeDivAlerta)
{

    try
    {

        var codCOmbo;
        var TextoAlerta;

        TextoAlerta = $(NomeCombo).attr('MsgObrigatorio');

        if ((typeof TextoAlerta === 'undefined') || (null == TextoAlerta) || (TextoAlerta == "")) {
            TextoAlerta = "Required Field!";
        }
        else
        {
            TextoAlerta = "Select '" + TextoAlerta + "'";
        }

        codCOmbo = $(NomeCombo + ' option:selected').val();
        if ((typeof codCOmbo === 'undefined') || (null == codCOmbo) || (codCOmbo == 0))
        {
            MostrarMensagem(NomeDivAlerta, TextoAlerta, TipoAviso.Aviso);
            $(NomeCombo).focus();
            return false;
        }
        return true;

    }
    catch (err)
    {
        alert('Error while selection: ' + err.description);
        return false;
    }

}

//function validate(form_id,email)
// { 
//         var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
// 
//        var address = document.forms[form_id].elements[email].value; if(reg.test(address) == false)
//       {  
//        alert('Invalid Email Address'); return false; 
//       }
// }




function checkemail(NomeText, NomeDivAlerta)
{ 
    try
    {

        var valTextbox;
        var TextoAlerta;

        TextoAlerta = $(NomeText).attr('MsgObrigatorio');

        if ((typeof TextoAlerta === 'undefined') || (null == TextoAlerta) || (TextoAlerta == ""))
        {
            TextoAlerta = "Please insert Text!";
        }
        else
        {
            TextoAlerta = TextoAlerta;
        }

        valTextbox = $(NomeText).val();
        
        var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
 
        if(reg.test(valTextbox) == false)
        {
          MostrarMensagem(NomeDivAlerta, TextoAlerta, TipoAviso.Aviso);
          $(NomeText).focus().select();
          return false;
                 
        }
        
        if ((typeof valTextbox === 'undefined') || (null == valTextbox) || (valTextbox == ''))
        {
            MostrarMensagem(NomeDivAlerta, TextoAlerta, TipoAviso.Aviso);
            $(NomeText).focus().select();
            return false;
        }

        return true;

    }
    catch (err)
    {
        alert('Please check Text: ' + err.description);
        return false;
    }



}

