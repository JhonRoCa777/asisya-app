type RouterNode = {
  link: string;
  fullPath?: string;
  [key: string]: any;
};

export const ROUTER: RouterNode = {
  link: '',
  PUBLIC: {
    link: '/login',
  },
  PRIVATE: {
    link: '/home',
    PRODUCTS: {
      link: '/productos',
    },
    CATEGORIES: {
      link: '/categorias',
    },
  }
};

function addFullPath(node: RouterNode, parentPath = '') {
  
  node.fullPath = parentPath + node.link;

  for (const key of Object.keys(node)) {
    const value = node[key];
    if (value && typeof value === 'object' && 'link' in value)
      addFullPath(value, node.fullPath);
  }
}

addFullPath(ROUTER);