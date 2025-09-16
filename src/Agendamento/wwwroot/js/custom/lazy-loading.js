
var lazyLoadInstance = new LazyLoad({
    elements_selector: ".lazy img",
    callback_load: (el) => {
        console.log("Imagem carregada: ", el);
    },
    callback_error: (el) => {
        console.error("Erro ao carregar imagem: ", el);
    }
});