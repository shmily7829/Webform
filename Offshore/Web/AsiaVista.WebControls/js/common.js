
function OneClickButton_Click( btn, onClientClick)
{
    btn.style.display = 'none';
    btn.nextSibling.style.display = '';
    var f = new Function(onClientClick);
    try{
        var r = f();
        if( r != null){ if( ! r ){
            btn.style.display = '';
            btn.nextSibling.style.display = 'none';
            return false;
        }}
    }catch(e){
        btn.style.display = '';
        btn.nextSibling.style.display = 'none';
        //window.status = e.description;
		alert(e.description);
        return false;
    }
    return true;
}


function blur_NumericBox( box, precision, scale, maxValue, minValue)
{
	if( box.value == '')	return false;

	if( isNaN( box.value))
	{
		alert( "Please input a numeric.");
		box.value = '';
		box.focus();
		return false;
	}

	var n = parseFloat( box.value);
	box.value = n;

	if( n > maxValue)
	{
		alert( "The max value is " + maxValue + ".");
		box.value = maxValue;
		box.focus();
		return false;
	}
	if( n < minValue)
	{
		alert( "The min value is " + minValue + ".");
		box.value = minValue;
		box.focus();
		return false;
	}




	var a = box.value.split('.');
	precision -= scale;

	var n0 = a[0].length;
	if( a[0].substring( 0, 1) == '-')	n0--;

	if( n0 > precision && a[0] != '0')
	{
		alert( "Overflowed, please input a numeric( " + precision + "." + scale + " )");
		box.value = "";
		box.focus();
		return false;
	}

	if( a.length == 2)
	{

		if( scale == 0)
		{
			alert( "The dot is not allowed.");
			box.value = "";
			box.focus();
			return false;
		}


		var n1 = a[1].length;

		if( n1 > scale)
		{
			alert( "Overflowed, please input a numeric( " + precision + "." + scale + " )");
			box.value = "";
			box.focus();
			return false;
		}

	}

	return true;
};


