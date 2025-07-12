(function ($) {
	"use strict";

	// LocalStorage JS
	let html = document.documentElement;
	let geexTheme = localStorage.theme;
	let geexThemeLayout = localStorage.layout;
	let geexThemeNavbar = localStorage.navbar;

	let darkMode = geexTheme === "dark";
	let rtlLayout = geexThemeLayout === "rtl";
	let topMenu = geexThemeNavbar === "top";

	// Theme Mode Toggle 
	if (geexTheme) {
		html.setAttribute("data-theme", geexTheme);

		if (geexTheme === "dark") {
			localStorage.theme === "dark"
			$(".geex-customizer__btn--light").removeClass("active");
			$(".geex-customizer__btn--dark").addClass("active");

		} else {
			localStorage.theme === "light"
		}
	}

	// Theme Layout Toggle
	if (geexThemeLayout) {

		html.setAttribute("dir", geexThemeLayout);

		if (geexThemeLayout === "rtl") {
			localStorage.themeLayout === "rtl"
			$(".geex-customizer__btn--ltr").removeClass("active");
			$(".geex-customizer__btn--rtl").addClass("active");
		} else {
			localStorage.themeLayout === "ltr"
		}
	}

	// Theme Navbar Toggle
	if (geexThemeNavbar) {

		html.setAttribute("data-nav", geexThemeNavbar);

		if (geexThemeNavbar === "top") {
			localStorage.themeNavbar === "top"
			$(".geex-customizer__btn--side").removeClass("active");
			$(".geex-customizer__btn--top").addClass("active");
		} else {
			localStorage.themeNavbar === "side"
		}
	}

	// Dark Theme
	function darkTheme(e) {
		let geexTheme = "dark";
		localStorage.theme = geexTheme;
		document.documentElement.setAttribute("data-theme", geexTheme);

		darkMode = true;
	}

	// Light Theme 
	function lightTheme(e) {
		let geexTheme = "light";
		localStorage.theme = geexTheme;
		document.documentElement.setAttribute("data-theme", geexTheme);

		darkMode = false;
	}

	// RTL Layout
	function rtlTheme(e) {
		let geexThemeLayout = "rtl";
		localStorage.layout = geexThemeLayout;
		document.documentElement.setAttribute("dir", geexThemeLayout);

		rtlLayout = true;
	}

	// LTR Layout
	function ltrTheme(e) {
		let geexThemeLayout = "ltr";
		localStorage.layout = geexThemeLayout;
		document.documentElement.setAttribute("dir", geexThemeLayout);

		rtlLayout = false;
	}

	// Top Navbar
	function topTheme(e) {
		let geexThemeNavbar = "top";
		localStorage.navbar = geexThemeNavbar;
		document.documentElement.setAttribute("data-nav", geexThemeNavbar);

		topMenu = true;
	}

	// Side Navbar
	function sideTheme(e) {
		let geexThemeNavbar = "side";
		localStorage.navbar = geexThemeNavbar;
		document.documentElement.setAttribute("data-nav", geexThemeNavbar);

		topMenu = false;
	}

	// Light Demo Add
	$(".geex-customizer__btn--light").click(function () {
		$(".geex-customizer__btn--dark").removeClass("active");
		$(".geex-customizer__btn--light").addClass("active");

		lightTheme();
	})

	// Dark Demo Add
	$(".geex-customizer__btn--dark").click(function () {
		$(".geex-customizer__btn--light").removeClass("active");
		$(".geex-customizer__btn--dark").addClass("active");

		darkTheme();
	})

	// LTR Layout Add
	$(".geex-customizer__btn--ltr").click(function () {
		$(".geex-customizer__btn--rtl").removeClass("active");
		$(".geex-customizer__btn--ltr").addClass("active");

		ltrTheme();

		// if($("body").hasClass("layout-rtl")) {
		// 	$("body").removeClass("layout-rtl");
		// }
		// $('html').attr('dir', 'ltr');
		// $("body").addClass("layout-ltr");
	})

	// RTL Layout Add
	$(".geex-customizer__btn--rtl").click(function () {
		$(".geex-customizer__btn--ltr").removeClass("active");
		$(".geex-customizer__btn--rtl").addClass("active");

		rtlTheme();
	})

	// Side Navbar Add
	$(".geex-customizer__btn--side").click(function () {
		$(".geex-customizer__btn--top").removeClass("active");
		$(".geex-customizer__btn--side").addClass("active");

		sideTheme();
	})

	// Top Navbar Add
	$(".geex-customizer__btn--top").click(function () {
		$(".geex-customizer__btn--side").removeClass("active");
		$(".geex-customizer__btn--top").addClass("active");

		topTheme();
	})

	// Menu Active Class
	function addActiveClass(pageSlug) {
		let menuLinks = $('.geex-header__menu__link, .geex-sidebar__menu__link');
		menuLinks.removeClass('active');

		// Find the corresponding menu item and add the "active" class
		menuLinks.each(function () {
			let menuItemPath = $(this).attr('href');
			let menuItemName = menuItemPath.split('/').pop().split('.')[0];
			if (menuItemName === pageSlug || menuItemName + '#' === pageSlug) {
				let menuParent = $(this).closest('.has-children').find('ul').siblings('a');
				$(this).addClass('active');
				menuParent.addClass('active');
				menuParent.siblings('.geex-sidebar__submenu').slideDown();

			} else if (pageSlug === '' || pageSlug === '#') {
				$('.geex-header__menu__link').first().addClass('active');
				$('.geex-sidebar__menu__link').first().addClass('active');

				$('.geex-header__menu__link').first().siblings('.geex-header__submenu').find('.geex-header__menu__link').first().addClass('active');
				$('.geex-header__menu__link').first().siblings('.geex-header__submenu').slideDown();

				$('.geex-sidebar__menu__link').first().siblings('.geex-sidebar__submenu').find('.geex-sidebar__menu__link').first().addClass('active');
				$('.geex-sidebar__menu__link').first().siblings('.geex-sidebar__submenu').slideDown();
			}
		});
	}

	// Get the path
	let path = window.location.pathname;
	let pathSegments = path.split('/');
	let pageSlug = pathSegments[pathSegments.length - 1].split('.')[0];

	addActiveClass(pageSlug);

	$(".geex-sidebar__menu__link").click(function () {
		let $clickedItem = $(this);

		// Toggle active class and slideToggle for the clicked item's submenu
		$clickedItem.toggleClass("active");
		$clickedItem.siblings('.geex-sidebar__submenu').slideToggle();

		// Remove active class and slideUp for other menu items
		$(".geex-sidebar__menu__link").not($clickedItem).removeClass("active");
		$(".geex-sidebar__menu__link").not($clickedItem).siblings(".geex-sidebar__submenu").slideUp();
	})

	// Customizer Toggle
	$(".geex-btn__customizer").click(function () {
		$(".geex-customizer").toggleClass("active");
		$("body").addClass("overlay_active");
	});

	// Customizer Close
	$(".geex-customizer-overlay, .geex-btn__customizer-close").click(function () {
		$(".geex-customizer").removeClass("active");
		$("body").removeClass("overlay_active");
	})

	// Sidebar Toggle
	$(".geex-btn__toggle-sidebar").click(function (e) {
		e.preventDefault();
		$(".geex-sidebar").toggleClass("active");
		$(".geex-sidebar").animate({
			width: "toggle"
		});
		$("body").addClass("overlay_active");
	});

	// Sidebar Close
	$(".geex-sidebar__close").click(function (e) {
		e.preventDefault();
		$(".geex-sidebar").removeClass("active");
		$(".geex-sidebar").animate({
			width: "toggle"
		});
		$("body").removeClass("overlay_active");
	});

	// Datepicker Open
	$("#geex-content__filter__label").click(function () {
		$('#geex-content__filter__date').datepicker().datepicker('show');
	});

	// Content Toggle
	$(".geex-content__toggle__btn").click(function (e) {
		e.preventDefault();
		$(this).toggleClass("active");
		$(this).siblings(".geex-content__toggle__content").slideToggle();
	});

	// Task Sidebar Toggle
	$(".geex-btn__toggle-task").click(function (e) {
		e.preventDefault();
		$(this).toggleClass("active");
		$(".geex-content__todo__sidebar").slideToggle();
	});

	// Calendar Sidebar Toggle
	$(".geex-content__calendar__toggle").click(function (e) {
		e.preventDefault();
		$(this).toggleClass("active");
		$(".geex-content__calendar__sidebar").slideToggle();
	});

	// Chat Sidebar Toggle
	$(".geex-content__chat__toggle").click(function (e) {
		e.preventDefault();
		$(this).toggleClass("active");
		$(".geex-content__chat__sidebar").slideToggle();
	});

	// Chat Action Toggle
	$(".geex-content__chat__action__toggle__btn").click(function (e) {
		e.preventDefault();
		$(this).toggleClass("active");
		$(this).siblings(".geex-content__chat__action__toggle__content").slideToggle();
	});

	// Popup Toggle
	$(".geex-content__header__quickaction__link").click(function (e) {
		e.preventDefault();
		var $popup = $(this).siblings('.geex-content__header__popup');

		$popup.slideToggle();
		$(".geex-content__header__popup").not($popup).slideUp();
	});

	// Add Todo
	$(".geex-btn__add-modal").click(function () {
		$(".geex-content__modal__form").addClass("active");
		$("body").addClass("overlay_active");
	})

	// Close Todo
	$(".geex-content__modal__form__close").click(function () {
		$(".geex-content__modal__form").removeClass("active");
		$("body").removeClass("overlay_active");
	})

	// Chat Mute Toggle
	$(".geex-content__chat__header__filter__mute-btn").click(function (e) {
		e.preventDefault();
		$(this).toggleClass("active");
	})

	// Chat Search Toggle
	$(".geex-content__chat__header__filter__btn").click(function (e) {
		e.preventDefault();

		var $clickedItem = $(this);

		// Toggle the 'active' class on the clicked item
		$clickedItem.toggleClass("active");

		// Slide toggle the content of the clicked item
		$clickedItem.siblings(".geex-content__chat__header__filter__content").slideToggle();

		// Close the content of other items if they are open
		$(".geex-content__chat__header__filter__btn").not($clickedItem).removeClass("active");
		$(".geex-content__chat__header__filter__btn").not($clickedItem).siblings(".geex-content__chat__header__filter__content").slideUp();
	})

	// Toggle Input Type Password
	$(".toggle-password-type").click(function (e) {
		e.preventDefault();
		const input = $(this).siblings("input");

		if (input.attr('type') === 'password') {
			$(this).removeClass("uil-eye");
			$(this).addClass("uil-eye-slash");
			input.attr('type', 'text');
		} else {
			$(this).addClass("uil-eye");
			$(this).removeClass("uil-eye-slash");
			input.attr('type', 'password');
		}
	});

	// Invoice Chat Toggle
	$(".geex-content__invoice__chat__toggler").click(function (e) {
		e.preventDefault();
		var $invoiceChatContent = $(this).siblings('.geex-content__invoice__chat__wrapper');

		$invoiceChatContent.stop().animate({
			width: 'toggle', // toggles between 0% and 100%
			opacity: 'toggle' // toggles between 0 and 1
		}, 300); // Adjust the duration as needed
	});

	// CountDown
	let day = document.querySelector('.geex-countdown__days');
	let hour = document.querySelector('.geex-countdown__hours');
	let minute = document.querySelector('.geex-countdown__minutes');
	let second = document.querySelector('.geex-countdown__seconds');

	function setCountdown() {

		// Set countdown date
		let countdownDate = new Date('Jan 01, 2025 16:40:25').getTime();

		// Update countdown every second
		let updateCount = setInterval(function () {

			// Get today's date and time
			let todayDate = new Date().getTime();

			// Get distance between now and countdown date
			let distance = countdownDate - todayDate;

			let days = Math.floor(distance / (1000 * 60 * 60 * 24));

			let hours = Math.floor(distance % (1000 * 60 * 60 * 24) / (1000 * 60 * 60));

			let minutes = Math.floor(distance % (1000 * 60 * 60) / (1000 * 60));

			let seconds = Math.floor(distance % (1000 * 60) / 1000);

			// Display values in html elements
			if (day) {
				day.textContent = days;
			}
			if (hour) {
				hour.textContent = hours;
			}
			if (minute) {
				minute.textContent = minutes;
			}
			if (second) {
				second.textContent = seconds;
			}

			// if countdown expires
			if (distance < 0) {
				clearInterval(updateCount);
				let countdownEl = document.getElementById("geex-countdown");
				if (countdownEl) {
					countdownEl.innerHTML = '<h1>EXPIRED</h1>';
				}
			}
		}, 300)
	}

	setCountdown()

	// Swiper Slider
	let slideCount = document.querySelectorAll('.swiper-container .swiper-slide').length;
	let enableLoop = slideCount > 3; // Asegura que hay suficientes slides
	let swiperContainer = document.querySelector('.swiper-container');
	let swiper = swiperContainer && new Swiper(swiperContainer, {
		loop: enableLoop, // Enable loop mode for infinite sliding
		freeMode: true,
		reverseDirection: true,
		slidesPerView: 3,
		spaceBetween: 0,
		rtl: true,
		navigation: {
			nextEl: '.swiper-btn-next',
			prevEl: '.swiper-btn-prev',
		},
		breakpoints: {
			// when window width is >= 0px
			0: {
				slidesPerView: 1,
			},
			// when window width is >= 1440px
			992: {
				slidesPerView: 2,
			},
			// when window width is >= 1600px
			1600: {
				slidesPerView: 3,
			}
		}
	});

	let testiContainer = document.querySelector('.testi-container');
	let testi = testiContainer && new Swiper(testiContainer, {
		loop: true, // Enable loop mode for infinite sliding
		freeMode: true,
		reverseDirection: true,
		slidesPerView: 1,
		spaceBetween: 0,
		navigation: {
			nextEl: '.swiper-btn-next',
			prevEl: '.swiper-btn-prev',
		},
	});

	// Editor
	if ($('.geex-content__chat__editor').length) {
		tinymce.init({
			selector: '.geex-content__chat__editor',  // change this value according to your HTML
		});
	}

	// Calendar
	if ($("#geex-calendar").length) {
		$('#geex-calendar').fullCalendar({
			themeSystem: 'jquery-ui',
			navLinks: true,
			height: 650,
			header: {
				left: 'title, prev, next',
				right: 'month,agendaWeek,agendaDay'
			},
			eventLimit: true, // allow "more" link when too many events
			eventSources: [
				{
					url: 'https://fullcalendar.io/demo-events.json'
				}
			],
			eventRender: function (event, element, view) {
				var theDate = event.start
				var endDate = event.dowend;
				var startDate = event.dowstart;

				if (theDate >= endDate) {
					return false;
				}

				if (theDate <= startDate) {
					return false;
				}
			},
			events: [
				{
					id: 1,
					title: "Lunch",
					start: '12:00',
					end: '12:30',
					dow: [1, 2, 3, 4, 5],
					dowstart: moment('2018-04-01', 'YYYY-MM-DD'),
					dowend: moment('2018-05-01', 'YYYY-MM-DD'),
					className: 'fc-event-lunch'
				}
			],
			businessHours: [{
				dow: [1, 2, 3, 4, 5],
				start: '08:00',
				end: '16:30'
			}],
			minTime: '06:00',
			maxTime: '21:00'
		});
	}
})(jQuery);