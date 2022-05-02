let produtos, vendas;

const obtemProdutos = async () => {
    const produtosResponse = await fetch('./api/produtos');
    return produtosResponse.ok ? await produtosResponse.json() : [];
}

const obtemVendas = async () => {
    const vendasResponse = await fetch('./api/vendas');
    return vendasResponse.ok ? await vendasResponse.json() : [];
}

const atualizaListaProdutos = async () => {
    produtos = await obtemProdutos();

    const listaProdutos = document.getElementById("lista-produtos").tBodies[0];
    const selectProdutos = document.getElementById("produto-id");
    listaProdutos.innerHTML = "";
    Array.from(produtos).forEach(p => {
        const marcacaoProduto = `<tr><td>${p.id}</td><td>${p.descricao}</td><td>${p.precobrl}</td></tr>`;
        listaProdutos.insertAdjacentHTML('beforeend', marcacaoProduto);

        const optionProduto =  `<option value="${p.id}">${p.descricao} - ${p.precobrl}</option>`;
        selectProdutos.insertAdjacentHTML('beforeend', optionProduto);
    });
}
const atualizaListaVendas = async () => {
    vendas = await obtemVendas();

    const listaVendas = document.getElementById("lista-vendas").tBodies[0];
    listaVendas.innerHTML = "";
    Array.from(vendas).forEach(v => {
        const marcacaoVenda = `<tr><td>${v.id}</td><td>${v.produto_id} {v.produto.descricao}</td><td>${v.precobrl}</td><td>${v.cotacaousd}</td><td>${v.taxabrl}</td><td>${v.totalbrl}</td></tr>`;
        listaVendas.insertAdjacentHTML('beforeend', marcacaoVenda);
    });
}

const atualizaCotacao = async () => {
    const response = await fetch('https://economia.awesomeapi.com.br/json/last/USD-BRL');
    if (response.ok) {
        const cotacao = document.getElementById("cotacao-atual");
        const dados = await response.json();
        cotacao.innerHTML = `1 BRL = ${dados.USDBRL.bid} USD`;
    }
};

document.addEventListener('DOMContentLoaded', async () => {
    await atualizaListaProdutos();
    await atualizaCotacao();
    
    setInterval(atualizaCotacao, 5000);
    setInterval(atualizaListaVendas, 2000);

    document.querySelectorAll('.atualizar-lista-produtos').forEach(el => el.addEventListener('click', () => atualizaListaProdutos()));
    document.querySelectorAll('.atualizar-lista-vendas').forEach(el => el.addEventListener('click', () => atualizaListaVendas()));
    document.querySelectorAll('.atualizar-cotacao').forEach(el => el.addEventListener('click', () => atualizaCotacao()));
});