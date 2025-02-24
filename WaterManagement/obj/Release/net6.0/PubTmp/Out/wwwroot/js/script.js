/* 
 * All functions s
 */

/*Inicialize all scripts on */
$(function(){
    $(".drawer").drawer();
    
    $("._homepage_slider").slick({
        infinite: true,
        dots: false,
        speed: 500,
        autoplay: true,
        slidesToShow: 1,
        sliteToScroll: 1
    }); 
    
    $(".sliderCartoes").slick({
        dots: false,
        infinte: true,
        fade: true,
        arrows: false,
        cssEase: 'linear',
        autoplay: true,
        speed: 700
    });
  
    $(".slider-seguros").slick({
        slidesToShow: 6,
        dots: true,
        arrows: true,
        autoplay: true,
        speed: 700,
        responsive: [
            {
                breakpoint: 1192,
                settings: {
                    arrows: true,
                    centerMode: true,
                    slidesToShow:  4
                }
            },
            {
                breakpoint: 768,
                settings: {
                    arrows: false,
                    centerMode: true,
                    slidesToShow:  1
                }
            },
            {
                breakpoint: 480,
                centerMode: true,
                centerPadding: '40px',
                slideToShow: 1
            }
        ]
    });
    
    $("a._image_hover").fancybox();
    
    $(".sliderAdvantagesContent").slick({
        dots: true,
        centerMode: false,
        slidesToShow: 6,
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    arrows: false,
                    centerMode: true,
                    slidesToShow:  1
                }
            },
            {
                breakpoint: 480,
                centerMode: true,
                centerPadding: '40px',
                slideToShow: 1
            }
        ]
        
    });
    
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover();
    
    acaoMoveUp();

    setTimeout(function () {
        $("#preloading").fadeOut().addClass("animated fadeOutBig"); 
    }, 2500);

    setTimeout(function () {
        $("#metodosPrevencao").modal({
            backdrop: "static",
            keyboard: true,
            show: true
        });
    }, 2500);
	
	setTimeout(function() {
		$(".toaster").css({
			display: "block"
		}).addClass("animated slideInRight");
	}, 7500);
	
	$(".toaster-close").click(function(){
		$(".toaster").fadeOut("slow");
	});
});

$.fn.isInViewport = function(){
  var elementTop     = $(this).offset().top;
  var elementBottom  = elementTop + $(this).outerHeight();
  
  var viewportTop    = $(window).scrollTop();
  var viewportBottom = viewportTop + $(window).height();
  
  return elementBottom > viewportTop && elementTop < viewportBottom;
};

/*Inicializes all sprcrollable functions*/
$(window).on('resize scroll', function(){
    addClassTopMenu();
    mostraMoveUp();
    
    $(".odometer").each(function(){
       if ( $(this).isInViewport() ){
           
            setTimeout(function(){
                $(".odometer.first").html(399);
                $(".odometer.second").html(1349);
                $(".odometer.third").html(1000);
                $(".odometer.fourth").html(92);
                $(".odometer.fifth").html(3455);
            }, 0);
       } 
    });
    
    if($(window).scrollTop() > 100){
        $("._download_solucoes").css({display: "block"}).addClass("animated fadeInUp");
    } else {
        $("._download_solucoes").css({display: "none"}).fadeOut("slow");
    }
});

/*Function that adds class to a top menu*/
function addClassTopMenu(){
    
    var scrollTop = $(window).scrollTop();
    
    if(scrollTop > 300){
        $("._the_header").addClass("_the_header_transparent_background");
    } else {
        $("._the_header").removeClass("_the_header_transparent_background");
    }
    
}

/*Acao do move up*/
function acaoMoveUp(){
    $("#moveUp").click(function(){
        $("body, html").animate({scrollTop: 0}, 1000, "linear");
    });
}

/*Mostra o move to up*/
function mostraMoveUp(){
    if($(window).scrollTop() > 100){
        $("#moveUp").css({display: "block"}).addClass("animated zoomIn");
    } else{
        $("#moveUp").css({display: "none"}).fadeOut("slow");
    }
}

function alertMessage(message){
	swal({
		title: "",
		type: "info",
		text: message
	});
}