// https://vitepress.dev/reference/site-config
export default {
	title: "Forged",
	description: "A fast, strict, and strongly-typed data generator (faker) for C# powered by Source Generators.",
	base: "/forged/",
	lastUpdated: true,
	themeConfig: {
		// https://vitepress.dev/reference/default-theme-config
		nav: [
			{ text: "Guide", link: "/guide/quick-start" },
			{ text: "Generator Reference", link: "/guide/generators" },
			{ text: "Examples", link: "/guide/examples" },
		],

		sidebar: [
			{
				text: "Guide",
				items: [
					{ text: "Quick Start", link: "/guide/quick-start" },
					{ text: "Generator Reference", link: "/guide/generators" },
					{ text: "Modifiers & Extensions", link: "/guide/modifiers" },
					{ text: "Determinism & Localization", link: "/guide/localization" },
					{ text: "Examples", link: "/guide/examples" },
				],
			},
		],

		socialLinks: [
			{ icon: "github", link: "https://github.com/Atulin/Forged" },
		],
	},
};