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
            Texto: ""
        }
    }
}).mount('#vueContainer');
