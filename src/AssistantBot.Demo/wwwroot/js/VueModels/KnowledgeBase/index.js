

const { createApp } = Vue;
createApp({
    data() {
        return {
            Texto: "",
            isLoading: false,
            showingAlert: false,
        };
    },
    methods: {
        guardar() {
            this.Texto = "";
            this.showingAlert = false;
            this.isLoading = true;
            setTimeout(this.showAlert, 3000);
            
            
        },
        
        showAlert() {
            this.isLoading = false;
            this.showingAlert = true;
        },

        hidenAlert() {
            this.showingAlert = false;
        }

    },
    computed: {
        buttonDisabled() {
            return this.Texto === "";
        }
    }

}).mount('#vueContainer');


