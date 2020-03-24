
function gv_Sort( i, sortBy, sortWay, sortBtn) 
{
	if (document.forms[0][sortBy].value == i) {
		if (document.forms[0][sortWay].value == '0') {
			document.forms[0][sortWay].value = '1';
		}
		else {
			document.forms[0][sortWay].value = '0';
		}
	}
	else {
		document.forms[0][sortWay].value = '0';
		document.forms[0][sortBy].value = i;
	}

	__doPostBack( sortBtn, '');
};



//
function gv_CheckBox( gv_id, box_id)
{
	this.gv_id = gv_id;
	this.box_id = box_id;
	
	this.selectedBox = new Array();
	this.elseBox = new Array();
	
	this.selectedValue = '';
	this.elseValue = '';
	
	
	var i, s;
	var oElements = document.getElementsByTagName("INPUT");
	for( i = 0; i < oElements.length; i++)
	{
		if( oElements[i].type.toLowerCase() == 'checkbox')
		{
			//if( ! oElements[i].disabled)
			//{
				if( gv_IsChildColumn(oElements[i].id, gv_id, box_id) )
				{
					if( oElements[i].checked )
					{
						this.selectedBox[this.selectedBox.length] = oElements[i];
						s = oElements[i].value;
						if( s == 'on' || s == 'off'){
							if( oElements[i].nextSibling != null){
								s = oElements[i].nextSibling.innerHTML;
							}
						}
						this.selectedValue += ';' + s;
					}
					else
					{
						this.elseBox[this.elseBox.length] = oElements[i];
						s = oElements[i].value;
						if( s == 'on' || s == 'off'){
							if( oElements[i].nextSibling != null){
								s = oElements[i].nextSibling.innerHTML;
							}
						}
						this.elseValue += ';' + s;
					}
				}
			//}
		}
	}
	
	if( this.selectedValue != ''){
		this.selectedValue = this.selectedValue.substr(1);	
	}
	if( this.elseValue != ''){
		this.elseValue = this.elseValue.substr(1);	
	}
	
};



function gv_ReverseCheckBox(gv_id,ChildId) 
{
	var oElements = document.getElementsByTagName("INPUT");
	for( i=0; i<oElements.length; i++)
	{
		if( oElements[i].type == 'checkbox')
		{
			if( ! oElements[i].disabled)
			{
				if( gv_IsChildColumn(oElements[i].id, gv_id, ChildId) )
				{
					oElements[i].checked = ( ! oElements[i].checked);
				}
			}
		}
	}
}


function gv_IsChildColumn(id, gv_id, ChildId) 
{
	var sPattern ='^'+gv_id+'.*'+ChildId+'$'; 
	var oRegExp = new RegExp(sPattern);
	if(oRegExp.exec(id))
		return true;
	else
		return false;
}


