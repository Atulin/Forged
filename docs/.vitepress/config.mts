// https://vitepress.dev/reference/site-config
export default {
	title: "Forged",
	description: "A fast, strict, and strongly-typed data generator (faker) for C# powered by Source Generators.",
	base: "/forged/",
	themeConfig: {
		// https://vitepress.dev/reference/default-theme-config
		nav: [
			{ text: "Home", link: "/" },
			{ text: "Examples", link: "/markdown-examples" },
		],

		sidebar: [
			{
				text: "Examples",
				items: [
					{ text: "Markdown Examples", link: "/markdown-examples" },
					{ text: "Runtime API Examples", link: "/api-examples" },
				],
			},
		],

		socialLinks: [
			{ icon: "github", link: "https://github.com/Atulin/Forged" },
		],
	},
};
