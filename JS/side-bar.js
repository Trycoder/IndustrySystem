var isExtended = 0;
var height = 450;
var width = 210;
var slideDuration = 500;
var opacityDuration = 1500;

function extendContract(){
	
	if(isExtended == 0){
		
		sideBarSlide(0, height, 0, width);
		
		sideBarOpacity(0, 1);
        if(isExtended == 0){
            $('sideBarContents').style.display="block";
        }
		isExtended = 1;
		
		// make expand tab arrow image face left (inwards)
		$('sideBarTab').childNodes[0].src = $('sideBarTab').childNodes[0].src.replace(/(\.[^.]+)$/, '-active$1');
		//$('sideBarContents').style.display="none";
		
	}
	else{
		
		
		if(isExtended == 1){
		    sideBarSlide(height, 0, width, 0);
		
		    sideBarOpacity(1, 0);
		    (function(){
	            $('sideBarContents').style.display="none";
            }).delay(1150);

		    

            
        }
		isExtended = 0;
		
		// make expand tab arrow image face right (outwards)
		
		$('sideBarTab').childNodes[0].src = $('sideBarTab').childNodes[0].src.replace(/-active(\.[^.]+)$/, '$1');
	}

}

function sideBarSlide(fromHeight, toHeight, fromWidth, toWidth){
		var myEffects = new Fx.Styles('sideBarContents', {duration: slideDuration, transition: Fx.Transitions.linear});
		myEffects.custom({
			 'height': [fromHeight, toHeight],
			 'width': [fromWidth, toWidth]
		});
}

function sideBarOpacity(from, to){
		var myEffects = new Fx.Styles('sideBarContents', {duration: opacityDuration, transition: Fx.Transitions.linear});
		myEffects.custom({
			 'opacity': [from, to]
		});
}

function init(){
    if(isExtended == 0){
        $('sideBarContents').style.display="none";
    }
	$('sideBarTab').addEvent('click', function(){extendContract()});
}

window.addEvent('load', function(){init()});