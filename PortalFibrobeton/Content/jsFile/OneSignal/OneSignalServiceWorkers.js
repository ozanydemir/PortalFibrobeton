async function save() {
    try {
        // Save data to database

        // Service Worker registration
        if ('serviceWorker' in navigator) {
            const registration = await navigator.serviceWorker.register('service-worker.js');
            console.log('Service Worker registered with scope:', registration.scope);
            // Notification options
            const iconUrl = new URL('./Gp/Gp/pics/fibroFavicon.png', location.href).toString();
            const options = {
                body: 'Yeni tarama eklendi!',
                icon: iconUrl
            };
            // Send notification
            registration.showNotification('Tarama kaydı başarılı!', options);
        }
    } catch (err) {
        console.error('Save error:', err);
    }
}

// Button element
const saveButton = document.getElementById('save-button');

// Click event listener for the button
saveButton.addEventListener('click', async function () {
    await save(); // Call the save function
});