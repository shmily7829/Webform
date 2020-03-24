

function open_DatePicker( txtDate, strFocusDate, strTitle, strVDir)
{
	if( strVDir == ''){
		strVDir = '/';
	}else if( strVDir.substring( strVDir.length -1, strVDir.length) != '/'){
		strVDir = strVDir + '/';
	}

	var sWidth	= "240";
	var sHeight	= "260";
	var sTop	= event.clientY + document.body.scrollTop - 50;
	var sLeft	= event.clientX + document.body.scrollLeft - 50;
	if( strTitle == null)	strTitle = "Please select a date.";

	var sfeature = "dialogWidth:" + sWidth + "px;dialogHeight:" + sHeight + "px;border:thin;help:no;scrollbars:no;status:no;dialogTop:" + sTop + "px;dialogLeft:" + sLeft + "px;"

	var oArgs = new Array();
	if( txtDate.value != ''){
		oArgs.strDate = txtDate.value;
	}else{
		oArgs.strDate = strFocusDate;
	}
	oArgs.strTitle = strTitle;

	//var oRtn = window.showModalDialog( "Calendar\\Calendar.htm", oArgs, sfeature);
	var oRtn = window.showModalDialog( strVDir + "AsiaVista.WebControls/Calendar/Calendar.htm", oArgs, sfeature);
	if( oRtn.strDate != null){
		txtDate.value = oRtn.strDate;
	}

	return oRtn.strDate;
}



function blur_DateBox( box)
{
	var s = box.value;

	if( s.length == 0){
		return false;
	}
	if( s.length == 1){
		s = '0' + s;
	}
	if( s.length == 2){
		var m = (new Date).getMonth() + 1;
		s = m + s;
	}
	if( s.length == 3){
		s = '0' + s;
	}
	if( s.length == 4){
		s = (new Date).getYear() + s;
	}
	if( s.length == 5){
		s = '0' + s;
	}
	if( s.length == 6){
		s = '20' + s;
	}
	if( s.length == 8){
		box.value = s.substr( 0, 4) + '/' + s.substr( 4, 2) + '/' + s.substr( 6, 2);
	}

	s = format_Date( box.value);
	if( s != box.value){
		alert( "Please input a date.");
		box.value = '';
		box.focus();
		return false;
	}

	return true;
}



function format_Date( sDate, sChar)	
{
	var dDate = ( ( sDate == null) ? (new Date()) : (new Date( sDate)) );
	if( sChar == null)	sChar = "/";

	var n;
	sDate = dDate.getFullYear() + sChar;
	n = dDate.getMonth() + 1;
	sDate += ( ( n < 10 ) ? ("0" + n) : (n) ) + sChar;
	n = dDate.getDate();
	sDate += ( ( n < 10 ) ? ("0" + n) : (n) );

	return sDate;
}
