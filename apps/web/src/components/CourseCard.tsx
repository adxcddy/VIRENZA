import Link from "next/link";

type Props = {
  title: string;
  description: string;
  category: string;
  lessons: number;
  level: string;
};

export default function CourseCard({
  title,
  description,
  category,
  lessons,
  level,
}: Props) {
  return (
    <article className="course-card">
      <div className="course-icon">◆</div>
      <span className="course-category">{category}</span>
      <h3>{title}</h3>
      <p>{description}</p>

      <div className="course-meta">
        <span>{lessons} lessons</span>
        <span>{level}</span>
      </div>

      <Link href="/register" className="course-link">
        Explore course →
      </Link>
    </article>
  );
}
