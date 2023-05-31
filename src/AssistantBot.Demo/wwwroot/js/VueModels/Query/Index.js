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
            Texto: "Hola Vue!"
        }
    }
}).mount('#vueContainer');