
// ------- //
// ------- //
// ------- //
// ---Smooth Scroll---- //

document.addEventListener("DOMContentLoaded", () => {
    const elements = document.querySelectorAll('.scroll-reveal');

    const observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          entry.target.classList.add('visible');
        } else {
          entry.target.classList.remove('visible'); // Reset when out of view
        }
      });
    }, {
      threshold: 0.3,
    });

    elements.forEach(el => observer.observe(el));
  });

//   -------------//
//   -------------//
//   -------------//
//   -------------//
//   -------------//
//   -------------//
//   -------------//

document.addEventListener('DOMContentLoaded', function() {
    const menuToggle = document.querySelector('.menu-toggle');
    const navLinks = document.querySelector('.nav-links');
    
    menuToggle.addEventListener('click', function() {
        navLinks.classList.toggle('active');
    });
    
    // Close menu when a link is clicked (for mobile)
    document.querySelectorAll('.nav-links a').forEach(link => {
        link.addEventListener('click', function() {
            navLinks.classList.remove('active');
        });
    });
});
//Slider 
document.addEventListener('DOMContentLoaded', function() {
    const slides = document.querySelector('.hero-slides');
    const slideItems = document.querySelectorAll('.hero-slide');
    const prevBtn = document.querySelector('.prev-btn');
    const nextBtn = document.querySelector('.next-btn');
    const dots = document.querySelectorAll('.dot');
    
    let currentIndex = 0;
    const slideCount = slideItems.length;
    let autoScrollInterval;
    const intervalTime = 3000; // 5 seconds
    
    // Initialize slider
    function updateSlider() {
        slides.style.transform = `translateX(-${currentIndex * 100}%)`;
        
        // Update dots
        dots.forEach((dot, index) => {
            dot.classList.toggle('active', index === currentIndex);
        });
    }
    
    // Next slide
    function nextSlide() {
        currentIndex = (currentIndex + 1) % slideCount;
        updateSlider();
    }
    
    // Previous slide
    function prevSlide() {
        currentIndex = (currentIndex - 1 + slideCount) % slideCount;
        updateSlider();
    }
    
    // Start auto-scrolling
    function startAutoScroll() {
        autoScrollInterval = setInterval(nextSlide, intervalTime);
    }
    
    // Stop auto-scrolling when user interacts
    function stopAutoScroll() {
        clearInterval(autoScrollInterval);
    }
    
    // Event listeners
    nextBtn.addEventListener('click', () => {
        nextSlide();
        stopAutoScroll();
        startAutoScroll();
    });
    
    prevBtn.addEventListener('click', () => {
        prevSlide();
        stopAutoScroll();
        startAutoScroll();
    });
    
    // Dot navigation
    dots.forEach((dot, index) => {
        dot.addEventListener('click', () => {
            currentIndex = index;
            updateSlider();
            stopAutoScroll();
            startAutoScroll();
        });
    });
    
    // Keyboard navigation
    document.addEventListener('keydown', (e) => {
        if (e.key === 'ArrowRight') {
            nextSlide();
            stopAutoScroll();
            startAutoScroll();
        } else if (e.key === 'ArrowLeft') {
            prevSlide();
            stopAutoScroll();
            startAutoScroll();
        }
    });
    
    // Start auto-scroll on page load
    startAutoScroll();
    
    // Pause auto-scroll when mouse enters hero section
    document.querySelector('.hero').addEventListener('mouseenter', stopAutoScroll);
    
    // Resume auto-scroll when mouse leaves hero section
    document.querySelector('.hero').addEventListener('mouseleave', startAutoScroll);
    
    // Handle touch events for mobile
    let touchStartX = 0;
    let touchEndX = 0;
    
    document.querySelector('.hero').addEventListener('touchstart', (e) => {
        touchStartX = e.changedTouches[0].screenX;
        stopAutoScroll();
    });
    
    document.querySelector('.hero').addEventListener('touchend', (e) => {
        touchEndX = e.changedTouches[0].screenX;
        handleSwipe();
        startAutoScroll();
    });
    
    function handleSwipe() {
        const threshold = 50;
        if (touchEndX < touchStartX - threshold) {
            nextSlide(); // Swipe left
        } else if (touchEndX > touchStartX + threshold) {
            prevSlide(); // Swipe right
        }
    }
});

// Slider For Categories 
document.addEventListener('DOMContentLoaded', function() {
    const servicesScroll = document.querySelector('.services-scroll');
    const serviceCards = document.querySelectorAll('.service-card');
    const prevBtn = document.querySelector('.service-prev-btn');
    const nextBtn = document.querySelector('.service-next-btn');
    const dots = document.querySelectorAll('.service-dot');
    
    let currentIndex = 0;
    const cardWidth = 300; // Should match your .service-card min-width + margin
    const cardsToShow = 3; // Number of cards visible at once
    let autoScrollInterval;
    const intervalTime = 3000; // 5 seconds
    
    // Initialize slider
    function updateSlider() {
        const scrollPosition = -currentIndex * (cardWidth + 24); // 24 accounts for margin-right
        servicesScroll.style.transform = `translateX(${scrollPosition}px)`;
        
        // Update dots
        dots.forEach((dot, index) => {
            dot.classList.toggle('active', index === currentIndex);
        });
    }
    
    // Next set of cards
    function nextSlide() {
        const maxIndex = serviceCards.length - cardsToShow;
        currentIndex = Math.min(currentIndex + 1, maxIndex);
        updateSlider();
    }
    
    // Previous set of cards
    function prevSlide() {
        currentIndex = Math.max(currentIndex - 1, 0);
        updateSlider();
    }
    
    // Start auto-scrolling
    function startAutoScroll() {
        autoScrollInterval = setInterval(() => {
            const maxIndex = serviceCards.length - cardsToShow;
            if (currentIndex >= maxIndex) {
                currentIndex = 0;
            } else {
                nextSlide();
            }
            updateSlider();
        }, intervalTime);
    }
    
    // Stop auto-scrolling when user interacts
    function stopAutoScroll() {
        clearInterval(autoScrollInterval);
    }
    
    // Event listeners
    nextBtn.addEventListener('click', () => {
        nextSlide();
        stopAutoScroll();
        startAutoScroll();
    });
    
    prevBtn.addEventListener('click', () => {
        prevSlide();
        stopAutoScroll();
        startAutoScroll();
    });
    
    // Dot navigation
    dots.forEach((dot, index) => {
        dot.addEventListener('click', () => {
            currentIndex = index;
            updateSlider();
            stopAutoScroll();
            startAutoScroll();
        });
    });
    
    // Start auto-scroll on page load
    startAutoScroll();
    
    // Pause auto-scroll when mouse enters services section
    document.querySelector('.services-container').addEventListener('mouseenter', stopAutoScroll);
    
    // Resume auto-scroll when mouse leaves services section
    document.querySelector('.services-container').addEventListener('mouseleave', startAutoScroll);
    
    // Handle touch events for mobile
    let touchStartX = 0;
    let touchEndX = 0;
    
    servicesScroll.addEventListener('touchstart', (e) => {
        touchStartX = e.changedTouches[0].screenX;
        stopAutoScroll();
    });
    
    servicesScroll.addEventListener('touchend', (e) => {
        touchEndX = e.changedTouches[0].screenX;
        handleSwipe();
        startAutoScroll();
    });
    
    function handleSwipe() {
        const threshold = 50;
        if (touchEndX < touchStartX - threshold) {
            nextSlide(); // Swipe left
        } else if (touchEndX > touchStartX + threshold) {
            prevSlide(); // Swipe right
        }
    }
    
    // Update on window resize
    window.addEventListener('resize', function() {
        updateSlider();
    });
});

// Services Section In Home Page
// Add click functionality to category items
document.querySelectorAll('.category-plus').forEach(item => {
    item.addEventListener('click', function() {
        const categoryName = this.previousElementSibling.textContent;
        alert(`Filtering by: ${categoryName}`);
        // In a real implementation, you would filter the services here
    });
});



// 
// 
// 
// 
// Offers Slider

document.addEventListener('DOMContentLoaded', function() {
    const offersScroll = document.querySelector('.offers-scroll');
    const prevBtn = document.querySelector('.prev-btn');
    const nextBtn = document.querySelector('.next-btn');
    
    // Calculate scroll amount (width of one card + gap)
    const card = document.querySelector('.offer-card');
    const cardWidth = card.offsetWidth;
    const gap = 24; // 1.5rem in pixels
    
    prevBtn.addEventListener('click', function() {
        offersScroll.scrollBy({
            left: -(cardWidth + gap),
            behavior: 'smooth'
        });
    });
    
    nextBtn.addEventListener('click', function() {
        offersScroll.scrollBy({
            left: cardWidth + gap,
            behavior: 'smooth'
        });
    });
    
    // Update timer every second
    function updateTimers() {
        const timers = document.querySelectorAll('.timer-numbers');
        
        timers.forEach(timer => {
            // In a real implementation, you would calculate actual time remaining
            // This is just a placeholder for the demo
            const days = Math.floor(Math.random() * 90);
            const hours = Math.floor(Math.random() * 24);
            const minutes = Math.floor(Math.random() * 60);
            const seconds = Math.floor(Math.random() * 60);
            
            timer.textContent = `${days} يوم | ${hours} ساعة | ${minutes} دقيقة | ${seconds} ثانية`;
        });
    }
    
    setInterval(updateTimers, 1000);
});


