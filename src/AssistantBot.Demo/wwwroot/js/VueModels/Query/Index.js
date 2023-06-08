// Esto era en Vue 2
//const vm = new Vue({
//    el: '#vueContainer',
//    data: {
//        Texto: ''
//    }
//})

const { createApp } = Vue;

createApp({
    data() {
        return {
            Texto: "",
            textoIngresado: "",
            mostrarRespuesta: false,
            isLoading: false,

        };
    },
    computed: {
        isButtonDisabled() {
            return this.Texto === '';
        }
    },
    methods: {
        consultar() {
            this.textoIngresado = this.Texto;
            this.Texto = "";
            this.mostrarRespuesta = false;
            this.isLoading = true;
            setTimeout(this.showAlert, 3000);
        },

        showAlert() {
            this.isLoading = false;
            this.mostrarRespuesta = true;
        },

        closeAlert() {
            this.mostrarRespuesta = false;   
        }  
    }
}).mount("#vueContainer");

